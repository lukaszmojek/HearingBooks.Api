using EasySynthesis.Domain.ValueObjects.Syntheses;

namespace EasySynthesis.Api.Syntheses.DialogueSyntheses;

public class DialogueSynthesisDto
{
	public Guid Id { get; set; }
	public Guid RequestingUserId { get; set; }
	public DialogueSynthesisStatus Status { get; set; }
	public string Title { get; set; }
	public string DialogueText { get; set; }
	public string BlobContainerName { get; set; }
	public string BlobName { get; set; }
	public string Language { get; set; }
	public string FirstSpeakerVoice { get; set; }
	public string SecondSpeakerVoice { get; set; }
}