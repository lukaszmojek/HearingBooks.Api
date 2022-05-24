using EasySynthesis.Api.Syntheses.TextSyntheses;

namespace EasySynthesis.Contracts.TextSynthesis;

public class SendLiveNotificationAboutDialogueSynthesis
{
    public Guid UserId { get; set; }
    public TextSynthesisDto TextSynthesis { get; set; }
}