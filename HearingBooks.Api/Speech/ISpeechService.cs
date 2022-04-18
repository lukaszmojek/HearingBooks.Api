using HearingBooks.Api.Syntheses.RequestTextSynthesis;

namespace HearingBooks.Api.Speech;

public interface ISpeechService
{
    public Task<(string, string)> SynthesizeAudioAsync(string containerName, string requestId, TextSyntehsisRequest textSyntehsisRequest);
}