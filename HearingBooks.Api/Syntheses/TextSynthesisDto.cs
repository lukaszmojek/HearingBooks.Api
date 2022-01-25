using HearingBooks.Domain.ValueObjects.TextSynthesis;

namespace HearingBooks.Api.Syntheses;

public class TextSynthesisDto
{
    public Guid Id { get; set; }
    public Guid RequestingUserId { get; set; }
    public TextSynthesisStatus Status { get; set; }
    public string Title { get; set; }
    public string SynthesisText { get; set; }
    public string BlobContainerName { get; set; }
    public string BlobName { get; set; }
    public string Language { get; set; }
    public string Voice { get; set; }
    
    // public string Name { get; set; }
    // public int CharactersCount { get; set; }
    // public int SynthesisLengthInSeconds { get; set; }
}