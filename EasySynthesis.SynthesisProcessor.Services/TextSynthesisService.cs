using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Domain.Entities;
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

    public TextSynthesisService(ISpeechService speechService, ITextSynthesisRepository textSynthesisRepository, HearingBooksDbContext context, ISynthesisPricingService synthesisPricingService)
    {
        _speechService = speechService;
        _textSynthesisRepository = textSynthesisRepository;
        _context = context;
        _synthesisPricingService = synthesisPricingService;
    }

    public async Task<Guid> CreateRequest(TextSynthesisData data, User requestingUser, Guid requestId)
    {
        if (requestingUser.CanRequestDialogueSynthesis() is false)
        {
            throw new Exception($"Users of type {requestingUser.Type} cannot create TextSyntheses!");
        }

        var synthesisCharacterCount = data.TextToSynthesize.Length;
        var synthesisPrice = await _synthesisPricingService.GetPriceForSynthesis(
            SynthesisType.DialogueSynthesis,
            synthesisCharacterCount
        );

        if (requestingUser.HasBalanceToCreateRequest(synthesisPrice) is false)
        {
            throw new Exception($@"User with id {requestingUser.Id} and Balance of {requestingUser.Balance} 
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

            // var textSynthesisData = new TextSynthesisData(request.Title, containerName, synthesisFilePath);

            var textSynthesis = new TextSynthesis
            {
                Id = requestId,
                RequestingUserId = requestingUser.Id,
                Status = TextSynthesisStatus.Submitted,
                // TextSynthesisData = textSynthesisData
                Title = data.Title,
                SynthesisText = data.TextToSynthesize,
                BlobContainerName = containerName,
                BlobName = synthesisFileName,
                Voice = data.Voice,
                Language = data.Language,
                CharacterCount = data.TextToSynthesize.Length,
                DurationInSeconds = await AudioFileHelper.TryGettingDuration(synthesisFileName)
            };

            await _textSynthesisRepository.Insert(textSynthesis);
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
}