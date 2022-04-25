
using HearingBooks.Api.Syntheses.TextSyntheses.RequestTextSynthesis;

namespace HearingBooks.Api.Speech;

public interface ISpeechService
{
    public Task<(string, string)> SynthesizeAudioAsync(string containerName, string requestId, SyntehsisRequest syntehsisRequest);
}