using HearingBooks.Api.Speech;
using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.TextSynthesis;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;

namespace HearingBooks.Api.Syntheses;

public class TextSynthesisService
{
    private readonly ISpeechService _speechService;
    private readonly ITextSynthesisRepository _textSynthesisRepository;
    private readonly HearingBooksDbContext _context;

    public TextSynthesisService(ISpeechService speechService, ITextSynthesisRepository textSynthesisRepository, HearingBooksDbContext context)
    {
        _speechService = speechService;
        _textSynthesisRepository = textSynthesisRepository;
        _context = context;
    }

    public async Task<Guid> CreateRequest(TextSyntehsisRequest request)
    {
        var containerName = request.RequestingUserId.ToString();

        var requestId = Guid.NewGuid();

        string synthesisFilePath = "";
        string synthesisFileName;
        
        try
        {
            (synthesisFilePath, synthesisFileName) = await _speechService.SynthesizeAudioAsync(
                containerName,
                requestId.ToString(),
                request
            );

            var textSynthesisData = new TextSynthesisData(request.Title, containerName, synthesisFilePath);

            var textSynthesis = new TextSynthesis
            {
                Id = requestId,
                RequestingUserId = request.RequestingUserId,
                Status = TextSynthesisStatus.Submitted,
                // TextSynthesisData = textSynthesisData
                Title = request.Title,
                SynthesisText = request.TextToSynthesize,
                BlobContainerName = containerName,
                BlobName = synthesisFileName,
                Voice = request.Voice,
                Language = request.Language
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