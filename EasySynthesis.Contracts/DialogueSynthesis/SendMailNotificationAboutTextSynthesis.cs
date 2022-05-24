using EasySynthesis.Contracts.DialogueSynthesis;

namespace EasySynthesis.Contracts.TextSynthesis;

public class SendMailNotificationAboutDialogueSynthesis
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public Guid RequestId { get; set; }
    public DialogueSynthesisData TextSynthesisData { get; set; }
}