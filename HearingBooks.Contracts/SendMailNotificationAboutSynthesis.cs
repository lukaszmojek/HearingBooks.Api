namespace EasySynthesis.Contracts;

public class SendMailNotificationAboutSynthesis
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    public Guid RequestId { get; set; }
    public string SynthesisTitle { get; set; }
}