using SampleDotnetCleanArchitecture.Domain.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSecurity _accountSecurity;

    public AccountService(
        IAccountRepository accountRepository,
        IAccountSecurity accountSecurity
        )
    {
        _accountRepository = accountRepository;
        _accountSecurity = accountSecurity; 
    }

    public async Task<ICollection<Account>> ListAsync() => await _accountRepository.ListAsync();
    public async Task<Account?> GetAsync(string username) => await _accountRepository.GetByNameAsync(username);

    public async Task<Account> CreateAsync(string userName, string password, ICollection<string>? roles, ICollection<AccountClaims>? claims)
    {
        var existedUser = await GetAsync(userName);
        if (existedUser != null)
            throw new Exception("Invalid user!");

        var user = new Account(userName, password, roles, claims);

        user.Validate();

        var passwordHash = _accountSecurity.HashPassword(user, password);
        
        user.SetPassword(passwordHash);

        await _accountRepository.CreateAsync(user);
        return user;
    }

    public async Task DeleteAsync(string userName)
    {
        await _accountRepository.DeleteAsync(userName);
    }

    public AccountToken GenerateJwtToken(Account user)
    {
        return _accountSecurity.GenerateJwtToken(user);
    }

    public async Task<AccountToken> LoginAsync(string username, string password)
    {
        var user = await GetAsync(username);
        if (user == null)
            throw new DomainValidationException("Invalid username or password");

        var result = _accountSecurity.VerifyHashedPassword(user, user.PasswordHash, password);

        if (!result)
            throw new DomainValidationException("Invalid username or password");

        return _accountSecurity.GenerateJwtToken(user);
    }

    public async Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName)
    {
        await _accountRepository.DeleteClaimFromUserAsync(userName, claimName);
    }

    public async Task AddClaimToUserAsync(string userName, AccountClaims claimName)
    {
        await _accountRepository.AddClaimToUserAsync(userName, claimName);
    }
}
