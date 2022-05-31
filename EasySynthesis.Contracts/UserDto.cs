using EasySynthesis.Domain.ValueObjects.User;

namespace EasySynthesis.Api.Languages.GetLanguages;

public class UserDto
{
    public UserType Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public double Balance { get; set; }
    public virtual PreferenceDto Preference { get; set; }
}