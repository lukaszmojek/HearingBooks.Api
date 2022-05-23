namespace EasySynthesis.Api.Core.Configuration;

public interface ISynthesisProcessorConfiguration
{
    public string this[string key]
    {
        get;
    }
}