namespace EasySynthesis.Contracts.DialogueSynthesis;

public class DialogueSynthesisRequested
{
	public Guid UserId { get; set; }
	public Guid RequestId { get; set; }
	public DialogueSynthesisData DialogueSynthesisData { get; set; }
}