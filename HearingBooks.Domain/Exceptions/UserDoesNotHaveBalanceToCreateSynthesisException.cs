namespace EasySynthesis.Domain.Exceptions;

public class UserDoesNotHaveBalanceToCreateSynthesisException : Exception
{
    public UserDoesNotHaveBalanceToCreateSynthesisException(string message) : base(message)
    {
        
    }
}