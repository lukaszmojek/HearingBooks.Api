using System.Runtime.InteropServices;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.Exceptions;
using EasySynthesis.Domain.ValueObjects.Syntheses;
using EasySynthesis.Infrastructure;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Persistance;
using EasySynthesis.Services.Speech;

namespace EasySynthesis.Services;

public class TextSynthesisService
{
    private readonly ISpeechService _speechService;
    private readonly ITextSynthesisRepository _textSynthesisRepository;
    private readonly HearingBooksDbContext _context;
    private readonly ISynthesisPricingService _synthesisPricingService;
    private readonly IVoiceRepository _voiceRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IUserRepository _userRepository;

    public TextSynthesisService(ISpeechService speechService, ITextSynthesisRepository textSynthesisRepository, HearingBooksDbContext context, ISynthesisPricingService synthesisPricingService, IVoiceRepository voiceRepository, ILanguageRepository languageRepository, IUserRepository userRepository)
    {
        _speechService = speechService;
        _textSynthesisRepository = textSynthesisRepository;
        _synthesisPricingService = synthesisPricingService;
        _voiceRepository = voiceRepository;
        _languageRepository = languageRepository;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<TextSynthesis> CreateRequest(TextSynthesisData data, Guid requestingUserId, Guid requestId)
    {
        var requestingUser = await _userRepository.GetUserByIdAsync(requestingUserId);

        if (requestingUser.CanRequestDialogueSynthesis() is false)
        {
            throw new EasySynthesisUserCannotCreateSynthesisException($"Users of type {requestingUser.Type} cannot create TextSyntheses!");
        }

        var synthesisCharacterCount = data.TextToSynthesize.Length;
        var synthesisPrice = await _synthesisPricingService.GetPriceForSynthesis(
            SynthesisType.TextSynthesis,
            synthesisCharacterCount
        );

        if (requestingUser.HasBalanceToCreateRequest(synthesisPrice) is false)
        {
            throw new UserDoesNotHaveBalanceToCreateSynthesisException($@"User with id {requestingUser.Id} and Balance of {requestingUser.Balance} 
                tried to request TextSynthesis worth {synthesisPrice}");
        }
        
        var containerName = requestingUser.Id.ToString();


        string synthesisFilePath = "";
        string synthesisFileName;

        //TODO: Move to mapper
        var synthesisRequest = new SyntehsisRequest()
        {
            Title = data.Title,
            Voice = data.Voice,
            Language = data.Language,
            TextToSynthesize = data.TextToSynthesize
        };
        
        try
        {
            (synthesisFilePath, synthesisFileName) = await _speechService.SynthesizeTextAsync(
                containerName,
                requestId.ToString(),
                synthesisRequest
            );

            var language = await _languageRepository.GetBySymbol(data.Language);
            var voice = await _voiceRepository.GetVoiceByName(data.Voice);
            
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var durationInSeconds = isWindows
                ? data.TextToSynthesize.Split(' ').Length / 3
                : await AudioFileHelper.TryGettingDuration(synthesisFileName);
            
            var textSynthesis = new TextSynthesis
            {
                Id = requestId,
                User = requestingUser,
                Status = TextSynthesisStatus.Submitted,
                Title = data.Title,
                SynthesisText = data.TextToSynthesize,
                BlobContainerName = containerName,
                BlobName = synthesisFileName,
                Voice = voice,
                Language = language,
                CharacterCount = data.TextToSynthesize.Length,
                PriceInUsd = synthesisPrice,
                DurationInSeconds = durationInSeconds
            };

            requestingUser.Balance -= synthesisPrice;
            await _textSynthesisRepository.Insert(textSynthesis);
            await _context.SaveChangesAsync();

            return textSynthesis;
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
}