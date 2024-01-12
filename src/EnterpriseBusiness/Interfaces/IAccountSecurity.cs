using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces
{
    public interface IAccountSecurity
    {
        AccountToken GenerateJwtToken(Account user);
        string HashPassword(Account user, string password);
        bool VerifyHashedPassword(Account user, string passwordHash, string password);
    }
}
