using HearingBooks.Domain.DDD;
using HearingBooks.Domain.ValueObjects.Syntheses;

namespace HearingBooks.Domain.Entities;

public class DialogueSynthesis : Entity<Guid>
{
	public virtual User User { get; set; }
	public DialogueSynthesisStatus Status { get; set; }
	public string Title { get; set; }
	public string DialogueText { get; set; }
	public string BlobContainerName { get; set; }
	public string BlobName { get; set; }
	public virtual Language Language { get; set; }
	public virtual Voice FirstSpeakerVoice { get; set; }
	public virtual Voice SecondSpeakerVoice { get; set; }
	public int CharacterCount { get; set; }
	public int DurationInSeconds { get; set; }
	public double PriceInUsd { get; set; }
}