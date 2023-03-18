using System.Runtime.InteropServices;
using HearingBooks.Contracts.DialogueSynthesis;
using HearingBooks.Domain.Entities;
using HearingBooks.Domain.Exceptions;
using HearingBooks.Domain.ValueObjects.Syntheses;
using HearingBooks.Infrastructure;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;
using HearingBooks.SynthesisProcessor.Services.Speech;

namespace HearingBooks.SynthesisProcessor.Services;

public class DialogueSynthesisService
{
    private readonly ISpeechService _speechService;
    private readonly IDialogueSynthesisRepository _dialogueSynthesisRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IVoiceRepository _voiceRepository;
    private readonly HearingBooksDbContext _context;
    private readonly ISynthesisPricingService _synthesisPricingService;
    private readonly IUserRepository _userRepository;

    public static string LineSeparator = "---";
    
    public DialogueSynthesisService(ISpeechService speechService, IDialogueSynthesisRepository dialogueSynthesisRepository, HearingBooksDbContext context, ISynthesisPricingService synthesisPricingService, IUserRepository userRepository, ILanguageRepository languageRepository, IVoiceRepository voiceRepository)
    {
        _speechService = speechService;
        _dialogueSynthesisRepository = dialogueSynthesisRepository;
        _context = context;
        _synthesisPricingService = synthesisPricingService;
        _userRepository = userRepository;
        _languageRepository = languageRepository;
        _voiceRepository = voiceRepository;
    }
    
    public async Task<DialogueSynthesis> CreateRequest(DialogueSynthesisData data, Guid requestingUserId, Guid requestId)
    {
        var requestingUser = await _userRepository.GetUserByIdAsync(requestingUserId);
        ValidateUser(requestingUser);

        var synthesisCharacterCount = data.DialogueText.Length;
        var synthesisPrice = await _synthesisPricingService.GetPriceForSynthesis(
            SynthesisType.DialogueSynthesis,
            synthesisCharacterCount
        );

        ValidateUserBalance(requestingUser, (double) synthesisPrice);
        var containerName = requestingUser.Id.ToString();
        var synthesisFilePath = "";

        try
        {
            var dialogueText = DialogueProcessor.BuildDialogueText(data, LineSeparator);
            var synthesisRequest = BuildSynthesisRequest(data, dialogueText);
            (synthesisFilePath, var synthesisFileName) = await _speechService.SynthesizeSsmlAsync(containerName, Guid.NewGuid().ToString(), synthesisRequest);

            var dialogueSynthesis = await CreateDialogueSynthesis(requestingUser, data, synthesisPrice, synthesisCharacterCount, synthesisFileName, containerName, requestId);
            await SaveDialogueSynthesis(requestingUser, dialogueSynthesis, synthesisPrice);

            return dialogueSynthesis;
        }
        catch (Exception e)
        {
            //TODO: Log exception
            throw;
        }
        finally
        {
            if (!string.IsNullOrEmpty(synthesisFilePath))
            {
                File.Delete(synthesisFilePath);
            }
        }
    }

    private void ValidateUser(User requestingUser)
    {
        if (!requestingUser.CanRequestDialogueSynthesis())
        {
            throw new HearingBooksUserCannotCreateSynthesisException($"Users of type {requestingUser.Type} cannot create DialogueSyntheses!");
        }
    }

    private void ValidateUserBalance(User requestingUser, double synthesisPrice)
    {
        if (!requestingUser.HasBalanceToCreateRequest(synthesisPrice))
        {
            throw new UserDoesNotHaveBalanceToCreateSynthesisException($@"User with id {requestingUser.Id} and Balance of {requestingUser.Balance} 
                tried to request DialogueSynthesis worth {synthesisPrice}");
        }
    }

    private SyntehsisRequest BuildSynthesisRequest(DialogueSynthesisData data, string dialogueText)
    {
        return new SyntehsisRequest
        {
            Title = data.Title,
            Voice = data.SecondSpeakerVoice,
            Language = data.Language,
            TextToSynthesize = dialogueText
        };
    }

    private async Task<DialogueSynthesis> CreateDialogueSynthesis(User requestingUser, DialogueSynthesisData data, decimal synthesisPrice, int synthesisCharacterCount, string synthesisFileName, string containerName, Guid requestId)
    {
        var language = await _languageRepository.GetBySymbol(data.Language);
        var firstSpeakerVoice = await _voiceRepository.GetVoiceByName(data.FirstSpeakerVoice);
        var secondSpeakerVoice = await _voiceRepository.GetVoiceByName(data.SecondSpeakerVoice);
        var durationInSeconds = await CalculateDuration(data.DialogueText, synthesisFileName);

        return new DialogueSynthesis
        {
            Id = requestId,
            User = requestingUser,
            Status = DialogueSynthesisStatus.Submitted,
            Title = data.Title,
            DialogueText = data.DialogueText,
            BlobContainerName = containerName,
            BlobName = synthesisFileName,
            FirstSpeakerVoice = firstSpeakerVoice,
            SecondSpeakerVoice = secondSpeakerVoice,
            Language = language,
            CharacterCount = synthesisCharacterCount,
            DurationInSeconds = durationInSeconds,
            PriceInUsd = (double) synthesisPrice
        };
    }

    private async Task<int> CalculateDuration(string dialogueText, string synthesisFileName)
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        return isWindows
            ? dialogueText.Split(' ').Length / 3
            : await AudioFileHelper.TryGettingDuration(synthesisFileName);
    }

    private async Task SaveDialogueSynthesis(User requestingUser, DialogueSynthesis dialogueSynthesis, decimal synthesisPrice)
    {
        requestingUser.Balance = Math.Round(requestingUser.Balance - (double) synthesisPrice, 2);
        await _dialogueSynthesisRepository.Insert(dialogueSynthesis);
        await _context.SaveChangesAsync();
    }
}