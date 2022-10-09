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

        if (requestingUser.CanRequestDialogueSynthesis() is false)
        {
            throw new HearingBooksUserCannotCreateSynthesisException($"Users of type {requestingUser.Type} cannot create DialogueSyntheses!");
        }

        var synthesisCharacterCount = data.DialogueText.Length;
        var synthesisPrice = await _synthesisPricingService.GetPriceForSynthesis(
            SynthesisType.DialogueSynthesis,
            synthesisCharacterCount
        );

        if (requestingUser.HasBalanceToCreateRequest(synthesisPrice) is false)
        {
            throw new UserDoesNotHaveBalanceToCreateSynthesisException($@"User with id {requestingUser.Id} and Balance of {requestingUser.Balance} 
                tried to request DialogueSynthesis worth {synthesisPrice}");
        }

        var containerName = requestingUser.Id.ToString();

        var synthesisFilePath = "";
        
        try
        {
            var openingTags =
                $"<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" version=\"1.0\" xml:lang=\"{data.Language}\">";
            var closingTags = "</speak>";

            var linesWithTags = SplitDialogueIntoLines(data.DialogueText)
                .Select(line => $"{LineOpeningTagsForSpeaker(data, line.Item2)}{line.Item1}{LineClosingTagsForSpeaker()}")
                .Aggregate((current, next) => $"{current}{next}");

            var dialogueText = $"{openingTags}{linesWithTags}{closingTags}";

            //TODO: Move to mapper?
            var synthesisRequest = new SyntehsisRequest
            {
                Title = data.Title,
                Voice = data.SecondSpeakerVoice,
                Language = data.Language,
                TextToSynthesize = dialogueText
            };
            
            (synthesisFilePath, var synthesisFileName) = await _speechService.SynthesizeSsmlAsync(
                containerName,
                Guid.NewGuid().ToString(),
                synthesisRequest
            );

            var language = await _languageRepository.GetBySymbol(data.Language);
            var firstSpeakerVoice = await _voiceRepository.GetVoiceByName(data.FirstSpeakerVoice);
            var secondSpeakerVoice = await _voiceRepository.GetVoiceByName(data.SecondSpeakerVoice);
            
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var durationInSeconds = isWindows
                ? data.DialogueText.Split(' ').Length / 3
                : await AudioFileHelper.TryGettingDuration(synthesisFileName);
            
            var dialogueSynthesis = new DialogueSynthesis
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
                PriceInUsd = synthesisPrice
            };
            
            requestingUser.Balance = Math.Round(requestingUser.Balance - synthesisPrice, 2);
            await _dialogueSynthesisRepository.Insert(dialogueSynthesis);
            await _context.SaveChangesAsync();

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

    public IEnumerable<(string, int)> SplitDialogueIntoLines(string dialogueText) =>
        dialogueText
            .Split(LineSeparator)
            .Select((line, index) => (line.Trim(), index));


    private string LineOpeningTagsForSpeaker(DialogueSynthesisData data, int index)
    {
        var speaker = FirstSpeaker(index) ? data.FirstSpeakerVoice : data.SecondSpeakerVoice;

        return $"<voice name=\"{speaker}\"><prosody rate=\"0%\" pitch=\"0%\">";
    }
    
    private string LineClosingTagsForSpeaker()
    {
        return "</prosody></voice>";
    }
    
    private bool FirstSpeaker(int index) => index % 2 == 0;
}