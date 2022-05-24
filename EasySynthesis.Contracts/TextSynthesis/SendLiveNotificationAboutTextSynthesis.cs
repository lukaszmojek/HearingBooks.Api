using EasySynthesis.Api.Syntheses.TextSyntheses;

namespace EasySynthesis.Contracts.TextSynthesis;

public class SendLiveNotificationAboutTextSynthesis
{
    public Guid UserId { get; set; }
    public TextSynthesisDto TextSynthesis { get; set; }
}