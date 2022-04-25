namespace HearingBooks.Api.Speech;

public class SyntehsisRequest
{
	public string TextToSynthesize { get; set; }
	public string Language { get; set; }
	public string Voice { get; set; }
}