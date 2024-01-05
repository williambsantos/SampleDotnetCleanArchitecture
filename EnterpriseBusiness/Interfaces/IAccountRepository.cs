using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces
{
    public interface IAccountRepository
    {
        Task<ICollection<Account>> ListAsync();
        Task<Account?> GetByNameAsync(string username);
        Task<Account> CreateAsync(Account user);
        Task DeleteAsync(string userName);
        Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName);
        Task AddClaimToUserAsync(string userName, AccountClaims claimName);
    }
}
