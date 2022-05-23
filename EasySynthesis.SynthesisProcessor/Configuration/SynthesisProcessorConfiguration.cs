using Microsoft.Extensions.Configuration;

namespace EasySynthesis.Api.Core.Configuration;

public class SynthesisProcessorConfiguration : ISynthesisProcessorConfiguration
{
    private readonly IConfiguration _configuration;

    public string this[string key]
    {
        get => _configuration[key];
    }
    
    public SynthesisProcessorConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}