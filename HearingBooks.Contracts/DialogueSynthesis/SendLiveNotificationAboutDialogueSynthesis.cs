namespace HearingBooks.Contracts.DialogueSynthesis;

public class SendLiveNotificationAboutDialogueSynthesis
{
    public Guid UserId { get; set; }
    public DialogueSynthesisDto DialogueSynthesis { get; set; }
}