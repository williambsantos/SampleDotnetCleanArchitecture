using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<AccountToken> LoginAsync(string username, string password);
        Task<ICollection<Account>> ListAsync();
        Task<Account?> GetAsync(string username);
        Task<Account> CreateAsync(string username, string password, ICollection<string>? roles, ICollection<AccountClaims>? claims);
        Task DeleteAsync(string userName);
        Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName);
        Task AddClaimToUserAsync(string userName, AccountClaims claimName);
    }
}
