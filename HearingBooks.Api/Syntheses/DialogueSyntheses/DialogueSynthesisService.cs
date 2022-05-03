using HearingBooks.Api.Speech;
using HearingBooks.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;
using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.TextSynthesis;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;

namespace HearingBooks.Api.Syntheses.DialogueSyntheses;

public class DialogueSynthesisService
{
    private readonly ISpeechService _speechService;
    private readonly IDialogueSynthesisRepository _dialogueSynthesisRepository;
    private readonly HearingBooksDbContext _context;

    public static string LineSeparator = "---";
    
    public DialogueSynthesisService(ISpeechService speechService, IDialogueSynthesisRepository dialogueSynthesisRepository, HearingBooksDbContext context)
    {
        _speechService = speechService;
        _dialogueSynthesisRepository = dialogueSynthesisRepository;
        _context = context;
    }

    public async Task<Guid> CreateRequest(DialogueSyntehsisRequest request, User requestingUser)
    {
        var containerName = requestingUser.Id.ToString();

        var requestId = Guid.NewGuid();
        var synthesisFilePath = "";
        
        try
        {
            var openingTags =
                $"<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" version=\"1.0\" xml:lang=\"{request.Language}\">";
            var closingTags = "</speak>";

            var linesWithTags = SplitDialogueIntoLines(request.DialogueText)
                .Select(line => $"{LineOpeningTagsForSpeaker(request, line.Item2)}{line.Item1}{LineClosingTagsForSpeaker()}")
                .Aggregate((current, next) => $"{current}{next}");

            var dialogueText = $"{openingTags}{linesWithTags}{closingTags}";

            //TODO: Move to mapper?
            var synthesisRequest = new SyntehsisRequest
            {
                Title = request.Title,
                Voice = request.SecondSpeakerVoice,
                Language = request.Language,
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
                Title = request.Title,
                DialogueText = request.DialogueText,
                BlobContainerName = containerName,
                BlobName = synthesisFileName,
                FirstSpeakerVoice = request.FirstSpeakerVoice,
                SecondSpeakerVoice = request.SecondSpeakerVoice,
                Language = request.Language,
                CharacterCount = request.DialogueText.Length,
                LengthInSeconds = await AudioFileHelper.TryGettingDuration(synthesisFileName)
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


    private string LineOpeningTagsForSpeaker(DialogueSyntehsisRequest request, int index)
    {
        var speaker = FirstSpeaker(index) ? request.FirstSpeakerVoice : request.SecondSpeakerVoice;

        return $"<voice name=\"{speaker}\"><prosody rate=\"0%\" pitch=\"0%\">";
    }
    
    private string LineClosingTagsForSpeaker()
    {
        return "</prosody></voice>";
    }
    
    private bool FirstSpeaker(int index) => index % 2 == 0;
}