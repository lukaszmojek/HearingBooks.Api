namespace HearingBooks.SynthesisProcessor.Configuration;

public interface ISynthesisProcessorConfiguration
{
    public string this[string key]
    {
        get;
    }
}