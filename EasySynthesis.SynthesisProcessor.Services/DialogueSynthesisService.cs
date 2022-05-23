using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.Syntheses;
using EasySynthesis.Infrastructure;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Persistance;
using EasySynthesis.Services.Speech;

namespace EasySynthesis.Services;

public class DialogueSynthesisService
{
    private readonly ISpeechService _speechService;
    private readonly IDialogueSynthesisRepository _dialogueSynthesisRepository;
    private readonly HearingBooksDbContext _context;
    private readonly ISynthesisPricingService _synthesisPricingService;

    public static string LineSeparator = "---";
    
    public DialogueSynthesisService(ISpeechService speechService, IDialogueSynthesisRepository dialogueSynthesisRepository, HearingBooksDbContext context, ISynthesisPricingService synthesisPricingService)
    {
        _speechService = speechService;
        _dialogueSynthesisRepository = dialogueSynthesisRepository;
        _context = context;
        _synthesisPricingService = synthesisPricingService;
    }

    public async Task<Guid> CreateRequest(DialogueSynthesisData data, User requestingUser, Guid requestId)
    {
        if (requestingUser.CanRequestDialogueSynthesis() is false)
        {
            throw new Exception($"Users of type {requestingUser.Type} cannot create DialogueSyntheses!");
        }

        var synthesisCharacterCount = data.DialogueText.Length;
        var synthesisPrice = await _synthesisPricingService.GetPriceForSynthesis(
            SynthesisType.DialogueSynthesis,
            synthesisCharacterCount
        );

        if (requestingUser.HasBalanceToCreateRequest(synthesisPrice) is false)
        {
            throw new Exception($@"User with id {requestingUser.Id} and Balance of {requestingUser.Balance} 
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

            var dialogueSynthesis = new DialogueSynthesis()
            {
                Id = requestId,
                RequestingUserId = requestingUser.Id,
                Status = DialogueSynthesisStatus.Submitted,
                Title = data.Title,
                DialogueText = data.DialogueText,
                BlobContainerName = containerName,
                BlobName = synthesisFileName,
                FirstSpeakerVoice = data.FirstSpeakerVoice,
                SecondSpeakerVoice = data.SecondSpeakerVoice,
                Language = data.Language,
                CharacterCount = synthesisCharacterCount,
                DurationInSeconds = await AudioFileHelper.TryGettingDuration(synthesisFileName),
                PriceInUsd = synthesisPrice
            };
            
            await _dialogueSynthesisRepository.Insert(dialogueSynthesis);
            await _context.SaveChangesAsync();
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

        return requestId;
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