using EasySynthesis.Domain.DDD;
using EasySynthesis.Domain.ValueObjects.User;

namespace EasySynthesis.Domain.Entities;

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
    public double Balance { get; set; }
    public virtual Preference Preference { get; set; }

    public User()
    {
        
    }

    public bool CanRequestTextSynthesis() =>
        Type switch
        {
            UserType.PayAsYouGo => true,
            _ => false
        };
    
    public bool CanRequestDialogueSynthesis() =>
        Type switch
        {
            UserType.PayAsYouGo => true,
            _ => false
        };

    public bool CanTopUpAccount() =>
        Type switch
        {
            UserType.PayAsYouGo => true,
            _ => false
        };

    public bool HasBalanceToCreateRequest(double synthesisCost) => Balance >= synthesisCost;

    public bool ShouldGetEmailNotification() =>
        (Email, Preference.EmailNotificationsEnabled) switch
        {
            ("", true) => false,
            (_, true) => true,
            _ => false
        };
}