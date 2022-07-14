namespace EasySynthesis.Contracts.TextSynthesis;

public class TextSynthesisRequested
{
	public Guid UserId { get; set; }
	public Guid RequestId { get; set; }
	public TextSynthesisData TextSynthesisData { get; set; }
}