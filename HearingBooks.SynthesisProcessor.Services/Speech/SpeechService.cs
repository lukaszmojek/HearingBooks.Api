using HearingBooks.Common;
using HearingBooks.Services.Core.Storage;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;

namespace HearingBooks.SynthesisProcessor.Services.Speech;

public class SpeechService : ISpeechService
{
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storage;

    public SpeechService(IConfiguration configuration, IStorageService storage)
    {
        _configuration = configuration;
        _storage = storage;
    }
    
    public async Task<(string, string)> SynthesizeTextAsync(string containerName, string requestId, SyntehsisRequest syntehsisRequest)
    {
        return await SynthesizeBaseAsync(
            containerName, 
            requestId, 
            syntehsisRequest,
            (s, request) => CreateTextSynthesisAsync(s, request)
        );
    }
    
    public async Task<(string, string)> SynthesizeSsmlAsync(string containerName, string requestId, SyntehsisRequest syntehsisRequest)
    {
        return await SynthesizeBaseAsync(
            containerName, 
            requestId, 
            syntehsisRequest,
(s, request) => CreateSsmlSynthesisAsync(s, request)
        );
    }
    
    private async Task<(string, string)> SynthesizeBaseAsync(string containerName, string requestId, SyntehsisRequest syntehsisRequest, Func<string, SyntehsisRequest, Task<string>> createSynthesisMethodAsync)
    {
        try
        {
            var blobName = $"{syntehsisRequest.Title.Replace(' ', '_')}-{requestId}.wav";
            var localPath = await createSynthesisMethodAsync(blobName, syntehsisRequest);

            await UploadSynthesis(containerName, blobName, localPath);
            
            return (localPath, blobName);
        }
        catch (Exception e)
        {
            //TODO: Add logging
            throw;
        }
    }

    private async Task<string> CreateTextSynthesisAsync(string blobName, SyntehsisRequest syntehsisRequest)
    {
        var config = SpeechConfig.FromSubscription(
            _configuration[ConfigurationKeys.TextToSpeechSubscriptionKey],
            _configuration[ConfigurationKeys.TextToSpeechRegion]
        );
        
        // Note: if only language is set, the default voice of that language is chosen.
        config.SpeechSynthesisLanguage = syntehsisRequest.Language; // For example, "de-DE"
        // The voice setting will overwrite the language setting.
        // The voice setting will not overwrite the voice element in input SSML.
        config.SpeechSynthesisVoiceName = syntehsisRequest.Voice;

        // Create AudioConfig for to let the application know how to handle the synthesis
        var localPath = $"./{blobName}";
        using var audioConfig = AudioConfig.FromWavFileOutput(localPath);
        // Actual synthetizer instance for TTS
        using var synthesizer = new SpeechSynthesizer(config, audioConfig);

        await synthesizer.SpeakTextAsync(syntehsisRequest.TextToSynthesize);
        
        return localPath;
    }
    
    private async Task<string> CreateSsmlSynthesisAsync(string blobName, SyntehsisRequest syntehsisRequest)
    {
        var config = SpeechConfig.FromSubscription(
            _configuration[ConfigurationKeys.TextToSpeechSubscriptionKey],
            _configuration[ConfigurationKeys.TextToSpeechRegion]
        );

        // Create AudioConfig for to let the application know how to handle the synthesis
        var localPath = $"./{blobName}";
        
        using var audioConfig = AudioConfig.FromWavFileOutput(localPath);
        // Actual synthetizer instance for TTS
        using var synthesizer = new SpeechSynthesizer(config, audioConfig);

        await synthesizer.SpeakSsmlAsync(syntehsisRequest.TextToSynthesize);
        
        return localPath;
    }

    private async Task UploadSynthesis(string containerName, string blobName, string localPath)
    {
        var blobContainerClient = await _storage.GetBlobContainerClientAsync(containerName);
        
        await _storage.UploadBlobAsync(blobContainerClient, blobName, localPath);
    }
}