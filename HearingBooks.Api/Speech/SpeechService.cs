using HearingBooks.Api.Storage;
using HearingBooks.Api.Syntheses;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace HearingBooks.Api.Speech;

public class SpeechService : ISpeechService
{
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storage;

    public SpeechService(IConfiguration configuration, IStorageService storage)
    {
        _configuration = configuration;
        _storage = storage;
    }

    public async Task<(string, string)> SynthesizeAudioAsync(string containerName, string requestId, TextSyntehsisRequest textSyntehsisRequest)
    {
        try
        {
            var fileName = $"{requestId}.wav";
            var localPath = await CreateSynthesis(fileName, textSyntehsisRequest);

            await UploadSynthesis(containerName, fileName, localPath);
            
            return (localPath, fileName);
        }
        catch (Exception e)
        {
            //TODO: Add logging
            throw;
        }
    }
    
    private async Task<string> CreateSynthesis(string fileName, TextSyntehsisRequest textSyntehsisRequest)
    {
        var config = SpeechConfig.FromSubscription(
            _configuration[ConfigurationKeys.TextToSpeechSubscriptionKey],
            _configuration[ConfigurationKeys.TextToSpeechRegion]
        );
        
        // Note: if only language is set, the default voice of that language is chosen.
        config.SpeechSynthesisLanguage = textSyntehsisRequest.Language; // For example, "de-DE"
        // The voice setting will overwrite the language setting.
        // The voice setting will not overwrite the voice element in input SSML.
        config.SpeechSynthesisVoiceName = textSyntehsisRequest.Voice;

        // Create AudioConfig for to let the application know how to handle the synthesis
        var localPath = $"./{fileName}";
        using var audioConfig = AudioConfig.FromWavFileOutput(localPath);
        // Actual synthetizer instance for TTS
        using var synthesizer = new SpeechSynthesizer(config, audioConfig);

        await synthesizer.SpeakTextAsync(textSyntehsisRequest.TextToSynthesize);

        return localPath;
    }

    private async Task UploadSynthesis(string containerName, string fileName, string localPath)
    {
        var blobContainerClient = await _storage.GetBlobContainerClientAsync(containerName);
        
        await _storage.UploadBlobAsync(blobContainerClient, fileName, localPath);
    }
}