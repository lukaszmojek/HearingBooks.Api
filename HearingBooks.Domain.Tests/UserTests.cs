using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.User;
using Xunit;

namespace EasySynthesis.Domain.Tests;

public class UserTests
{
	[Theory]
	[InlineData(UserType.EasySynthesis, false)]
	[InlineData(UserType.PayAsYouGo, true)]
	public void CanRequestTextSynthesis_Should_Return_Correct_Value_For_UserType(UserType userType, bool expectedResult)
	{
		var user = new User { Type = userType };

		var result = user.CanRequestTextSynthesis();

		Assert.Equal(expectedResult, result);
	}
	
	[Theory]
	[InlineData(UserType.EasySynthesis, false)]
	[InlineData(UserType.PayAsYouGo, true)]
	public void CanRequestDialogueSynthesis_Should_Return_Correct_Value_For_UserType(UserType userType, bool expectedResult)
	{
		var user = new User {Type = userType};

		var result = user.CanRequestDialogueSynthesis();

		Assert.Equal(expectedResult, result);
	}
	
	[Theory]
	[InlineData(UserType.EasySynthesis, false)]
	[InlineData(UserType.PayAsYouGo, true)]
	public void CanTopUpAccount_Should_Return_Correct_Value_For_UserType(UserType userType, bool expectedResult)
	{
		var user = new User {Type = userType};

		var result = user.CanTopUpAccount();

		Assert.Equal(expectedResult, result);
	}
	
	[Theory]
	[InlineData(10, 4, true)]
	[InlineData(10, 10, true)]
	[InlineData(10, 20, false)]
	public void HasBalanceToCreateRequest_Should_Return_True_When_User_Has_Balance_And_False_When_Synthesis_Is_More_Expensive(
		double balance, double synthesisCost, bool expectedResult)
	{
		var user = new User { Balance = balance };

		var result = user.HasBalanceToCreateRequest(synthesisCost);

		Assert.Equal(expectedResult, result);
	}
	
	[Fact]
	public void ShouldGetEmailNotification_Should_Return_True_When_User_Email_Specified_And_Has_EmailNotificationsEnabled_Set_To_True()
	{
		var user = new User { 
			Email = "email@test.com", 
			Preference = new Preference()
			{
				EmailNotificationsEnabled = true
			} 
		};

		var result = user.ShouldGetEmailNotification();

		Assert.True(result);
	}
	
	[Fact]
	public void ShouldGetEmailNotification_Should_Return_False_When_User_Does_Not_Have_Email_Specified_And_Has_EmailNotificationsEnabled_Set_To_True()
	{
		var user = new User { Email = "", 
			Preference = new Preference()
			{
				EmailNotificationsEnabled = true
			}  };

		var result = user.ShouldGetEmailNotification();

		Assert.False(result);
	}
	
	[Fact]
	public void ShouldGetEmailNotification_Should_Return_False_When_User_Email_Specified_And_Has_EmailNotificationsEnabled_Set_To_False()
	{
		var user = new User { Email = "email@test.com",
			Preference = new Preference()
			{
				EmailNotificationsEnabled = false
			}  };

		var result = user.ShouldGetEmailNotification();

		Assert.False(result);
	}
}