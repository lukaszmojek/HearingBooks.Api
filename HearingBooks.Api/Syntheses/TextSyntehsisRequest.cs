namespace HearingBooks.Api.Syntheses;

public class TextSyntehsisRequest
{
    public string Title { get; set; }
    public string TextToSynthesize { get; set; }
    public string Language { get; set; }
    public string Voice { get; set; }
    public Guid RequestingUserId { get; set; }
}