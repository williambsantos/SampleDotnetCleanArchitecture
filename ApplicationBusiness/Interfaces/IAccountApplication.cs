using SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces
{
    public interface IAccountApplication
    {
        Task<AccountLoginResponseDto?> LoginAsync(AccountLoginRequestDto user);
        Task<ICollection<AccountResponseDto>> ListAsync();
        Task<AccountResponseDto?> GetByUserNameAsync(string userName);
        Task<AccountCreateResponseDto> CreateAsync(AccountCreateRequestDto request);
        Task DeleteAsync(string userName);
        Task AddClaimToUserAsync(string userName, AccountClaims claimName);
        Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName);
    }
}
