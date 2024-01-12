using FluentAssertions;
using Moq;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Tests.Services;

public class AccountServiceTest
{
    [Fact]
    public async Task Login_WhenMatched_Ok()
    {
        // Arrange
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        var accountToken = new AccountToken(account.UserName, "dasdadsadasdas", DateTime.Now.AddDays(10));

        accountSecurity
            .Setup(x => x.VerifyHashedPassword(account, account.PasswordHash, account.PasswordHash))
            .Returns(true);

        accountSecurity
            .Setup(x => x.GenerateJwtToken(account))
            .Returns(accountToken);

        accountRepository
            .Setup(x => x.GetByNameAsync(account.UserName))
            .ReturnsAsync(account);

        // Act
        var result = await actual.LoginAsync(account.UserName, account.PasswordHash);

        // Assert
        result.Should().BeEquivalentTo(accountToken);
    }

    [Fact]
    public void Login_WhenNotMatched_ThrowsException()
    {
        // Arrange
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        var accountToken = new AccountToken(account.UserName, "dasdadsadasdas", DateTime.Now.AddDays(10));

        accountRepository
            .Setup(x => x.GetByNameAsync(account.UserName))
            .ReturnsAsync(account);

        accountSecurity
            .Setup(x => x.VerifyHashedPassword(account, account.PasswordHash, account.PasswordHash))
            .Returns(false);

        accountSecurity
            .Setup(x => x.GenerateJwtToken(account))
            .Returns(accountToken);

        // Act
        actual.Invoking(x => x.LoginAsync(account.UserName, account.PasswordHash))
            .Should().ThrowAsync<DomainValidationException>()
            .WithMessage("Invalid username or password");
    }    

    [Fact]
    public void Login_WhenUserNotExists_ThrowsException()
    {
        // Arrange
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        var accountToken = new AccountToken(account.UserName, "dasdadsadasdas", DateTime.Now.AddDays(10));

        accountRepository
            .Setup(x => x.GetByNameAsync(account.UserName))
            .ReturnsAsync((Account?)null);

        accountSecurity
            .Setup(x => x.VerifyHashedPassword(account, account.PasswordHash, account.PasswordHash))
            .Returns(false);

        accountSecurity
            .Setup(x => x.GenerateJwtToken(account))
            .Returns(accountToken);

        // Act
        actual.Invoking(x => x.LoginAsync(account.UserName, account.PasswordHash))
            .Should().ThrowAsync<DomainValidationException>()
            .WithMessage("Invalid username or password");
    }

    [Fact]
    public async Task GetAsync_WhenCalled_Ok()
    {
        // Arrange
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        accountRepository
            .Setup(x => x.GetByNameAsync(account.UserName))
            .ReturnsAsync(account);

        // Act
        var result = await actual.GetAsync(account.UserName);

        // Assert
        result.Should().BeEquivalentTo(account);
    }

    [Fact]
    public async Task ListAsync_WhenCalled_Ok()
    {
        // Arrange
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        accountRepository
            .Setup(x => x.ListAsync())
            .ReturnsAsync(new List<Account> { account });

        // Act
        var result = await actual.ListAsync();

        // Assert
        result.Should().BeEquivalentTo(new List<Account> { account });
    }

    [Fact]
    public async Task CreateAsync_WhenFilledAndNotExists_CanCreate()
    {
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        accountRepository
            .Setup(x => x.GetByNameAsync(account.UserName))
            .ReturnsAsync((Account?)null);

        accountSecurity
            .Setup(x => x.HashPassword(It.IsAny<Account>(), It.IsAny<string>()))
            .Returns("passwordHashed");

        accountRepository
            .Setup(x => x.CreateAsync(account))
            .Returns(Task.CompletedTask);

        var result = await actual.CreateAsync(account.UserName, account.PasswordHash, account.Roles, account.Claims);

        result.Should().NotBeNull();
        result.UserName.Should().Be(account.UserName);
        result.PasswordHash.Should().NotBeNullOrWhiteSpace();
        result.PasswordHash.Should().NotBe(account.PasswordHash);
    }

    [Fact]
    public void CreateAsync_WhenFilledButExists_ThrowsException()
    {
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        accountSecurity
            .Setup(x => x.HashPassword(account, account.PasswordHash))
            .Returns(account.PasswordHash);

        accountRepository
            .Setup(x => x.GetByNameAsync(account.UserName))
            .ReturnsAsync(account);

        accountRepository
            .Setup(x => x.CreateAsync(account))
            .Returns(Task.CompletedTask);

        actual.Invoking(x => x.CreateAsync(account.UserName, account.PasswordHash, account.Roles, account.Claims))
            .Should().ThrowAsync<DomainValidationException>()
            .WithMessage("Invalid user!");
    }

    [Fact]
    public async Task DeleteAsync_WhenCalled_Ok()
    {
        // Arrange
        var accountRepository = new Mock<IAccountRepository>();
        var accountSecurity = new Mock<IAccountSecurity>();

        var actual = new AccountService(accountRepository.Object, accountSecurity.Object);

        var account = new Account("name", "password");

        accountRepository
            .Setup(x => x.DeleteAsync(account.UserName))
            .Returns(Task.CompletedTask);

        // Act
        await actual.DeleteAsync(account.UserName);

        // Assert
        accountRepository.Verify(x => x.DeleteAsync(account.UserName), Times.Once);
    }

    
}
