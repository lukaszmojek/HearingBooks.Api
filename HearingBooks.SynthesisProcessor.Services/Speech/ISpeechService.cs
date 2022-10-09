
namespace HearingBooks.SynthesisProcessor.Services.Speech;

public interface ISpeechService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="requestId"></param>
    /// <param name="syntehsisRequest"></param>
    /// <returns>
    /// First string being localPath to the file
    /// Second string being fileName of the file
    /// </returns>
    public Task<(string, string)> SynthesizeTextAsync(string containerName, string requestId, SyntehsisRequest syntehsisRequest);
    public Task<(string, string)> SynthesizeSsmlAsync(string containerName, string requestId, SyntehsisRequest syntehsisRequest);
}