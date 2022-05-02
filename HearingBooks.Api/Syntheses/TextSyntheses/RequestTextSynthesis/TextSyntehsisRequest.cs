namespace HearingBooks.Api.Syntheses.TextSyntheses.RequestTextSynthesis;

public class TextSyntehsisRequest
{
    public string Title { get; set; }
    public string TextToSynthesize { get; set; }
    public string Language { get; set; }
    public string Voice { get; set; }
}