using HearingBooks.Domain.DDD;
using HearingBooks.Domain.ValueObjects.TextSynthesis;
#pragma warning disable CS8618

namespace HearingBooks.Domain.Entities;

public class TextSynthesis : Entity<Guid>
{
	public Guid RequestingUserId { get; set; }
	public TextSynthesisStatus Status { get; set; }
	public string Title { get; set; }
	public string SynthesisText { get; set; }
	public string BlobContainerName { get; set; }
	public string BlobName { get; set; }
	public string Language { get; set; }
	public string Voice { get; set; }
	public int CharacterCount { get; set; }
	public int DurationInSeconds { get; set; }
	public double PriceInUsd { get; set; }
}
