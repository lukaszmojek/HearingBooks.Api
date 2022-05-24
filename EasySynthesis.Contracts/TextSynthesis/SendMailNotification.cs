namespace EasySynthesis.Contracts.TextSynthesis;

public class SendMailNotification
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public Guid RequestId { get; set; }
    public TextSynthesisData TextSynthesisData { get; set; }
}