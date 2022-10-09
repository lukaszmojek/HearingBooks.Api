namespace HearingBooks.Contracts.TextSynthesis;

public class TextSynthesisData
{
	public string Title { get; set; }
	public string TextToSynthesize { get; set; }
	public string Language { get; set; }
	public string Voice { get; set; }
}