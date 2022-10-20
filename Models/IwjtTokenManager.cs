namespace Demoproject.Models
{
    public interface IwjtTokenManager
    {
        string Authenticate(string Email, string Password);
    }
}
