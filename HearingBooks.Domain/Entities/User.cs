
using HearingBooks.Domain.DDD;
using HearingBooks.Domain.ValueObjects.User;

namespace HearingBooks.Domain.Entities;

public class User : Entity<Guid>
{
    public UserType Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    //TODO: Change Password to password hash
    public string Password { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
    public bool EmailIsUsername { get; set; }

    public User()
    {
        
    }

    public bool CanRequestTextSynthesis() =>
        Type switch
        {
            UserType.PayAsYouGo => true,
            UserType.Subscriber => true,
            UserType.Writer => true,
            _ => false
        };
    
    public bool CanRequestBookValidation() =>
        Type switch
        {
            UserType.Writer => true,
            _ => false
        };
    
    public bool ShouldGetEmailNotification() =>
        (Email, EmailNotificationsEnabled) switch
        {
            ("", true) => false,
            (_, true) => true,
            _ => false
        };
    
    public bool CanTopUpAccount() =>
        Type switch
        {
            UserType.PayAsYouGo => true,
            UserType.Subscriber => true,
            UserType.Writer => true,
            _ => false
        };
}