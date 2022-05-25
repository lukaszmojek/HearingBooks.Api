using EasySynthesis.Api.Syntheses.DialogueSyntheses;

namespace EasySynthesis.Contracts.TextSynthesis;

public class SendLiveNotificationAboutDialogueSynthesis
{
    public Guid UserId { get; set; }
    public DialogueSynthesisDto DialogueSynthesis { get; set; }
}