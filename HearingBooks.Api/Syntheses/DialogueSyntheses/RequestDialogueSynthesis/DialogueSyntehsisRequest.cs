namespace HearingBooks.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;

public class DialogueSyntehsisRequest
{
	public string Title { get; set; }
	public string DialogueText { get; set; }
	public string Language { get; set; }
	public string FirstSpeakerVoice { get; set; }
	public string SecondSpeakerVoice { get; set; }
}