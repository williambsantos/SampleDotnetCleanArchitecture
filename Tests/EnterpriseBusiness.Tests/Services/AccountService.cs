using Moq;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;

public class AccountServiceTest
{
    [Fact]
    public async Task CreateUserAsync_WhenCalled_Ok()
    {
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        accountSecurity.Setup(x => x.HashPassword(account, account.PasswordHash)).Returns(account.PasswordHash);
        accountRepository.Setup(x => x.CreateAsync(account)).ReturnsAsync(account);

        accountRepository.Setup(x => x.GetByNameAsync(account.UserName)).ReturnsAsync(account);
        accountRepository.Setup(x => x.CreateAsync(account)).ReturnsAsync(account);

        var result = await actual.CreateUserAsync(account.UserName, account.PasswordHash, account.Roles, account.Claims);

        Assert.Equal(account, result);
    }

    [Fact]
    public async Task Login_WhenCalled_Ok()
    {
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        var accountToken = new AccountToken(account.UserName, "dasdadsadasdas", DateTime.Now.AddDays(10));

        accountSecurity.Setup(x => x.VerifyHashedPassword(account, account.PasswordHash, account.PasswordHash)).Returns(true);
        accountSecurity.Setup(x => x.GenerateJwtToken(account)).Returns(accountToken);
        accountRepository.Setup(x => x.GetByNameAsync(account.UserName)).ReturnsAsync(account);

        var result = await actual.LoginAsync(account.UserName, account.PasswordHash);

        Assert.Equal(accountToken, result);
    }
}
