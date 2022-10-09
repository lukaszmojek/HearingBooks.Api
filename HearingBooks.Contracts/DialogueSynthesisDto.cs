using HearingBooks.Domain.ValueObjects.Syntheses;

namespace HearingBooks.Contracts;

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
	public int CharacterCount { get; set; }
	public int DurationInSeconds { get; set; }
	public int PriceInUsd { get; set; }
}