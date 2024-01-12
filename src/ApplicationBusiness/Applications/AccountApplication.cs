using SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces;
using SampleDotnetCleanArchitecture.Domain.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Applications
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountService _accountService;

        public AccountApplication(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<AccountLoginResponseDto?> LoginAsync(AccountLoginRequestDto user)
        {
            AccountToken? userToken = await _accountService.LoginAsync(user.Username, user.Password);
            if (null == userToken) return null;
            return new AccountLoginResponseDto(userToken);
        }

        public async Task<AccountResponseDto?> GetByUserNameAsync(string userName)
        {
            var user = await _accountService.GetAsync(userName);
            if (null == user) return null;
            return new AccountResponseDto(user);
        }
        public async Task<ICollection<AccountResponseDto>> ListAsync()
        {
            var users = await _accountService.ListAsync();
            if (null == users) return new List<AccountResponseDto>();
            return users.Select(u => new AccountResponseDto(u)).ToList();
        }

        public async Task<AccountCreateResponseDto> CreateAsync(AccountCreateRequestDto request)
        {
            var user = await _accountService.CreateAsync(request.Username, request.Password, request.Roles, request.Claims);
            return new AccountCreateResponseDto(user);
        }

        public async Task DeleteAsync(string userName)
        {
            await _accountService.DeleteAsync(userName);
        }

        public async Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName)
        {
            await _accountService.DeleteClaimFromUserAsync(userName, claimName);
        }

        public async Task AddClaimToUserAsync(string userName, AccountClaims claimName)
        {
            await _accountService.AddClaimToUserAsync(userName, claimName);
        }
    }
}
