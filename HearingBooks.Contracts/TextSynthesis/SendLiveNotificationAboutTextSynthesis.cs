namespace HearingBooks.Contracts.TextSynthesis;

public class SendLiveNotificationAboutTextSynthesis
{
    public Guid UserId { get; set; }
    public TextSynthesisDto TextSynthesis { get; set; }
}