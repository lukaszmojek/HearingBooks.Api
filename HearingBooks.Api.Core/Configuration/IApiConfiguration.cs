namespace EasySynthesis.Api.Core.Configuration;

public interface IApiConfiguration
{
    string JwtSecret();

    public string this[string key]
    {
        get;
    }
}