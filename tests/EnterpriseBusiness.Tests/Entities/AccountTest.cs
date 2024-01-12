using FluentAssertions;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Tests.Entities;

public class AccountTest
{
    [Theory]
    [InlineData("name")]
    public void Construct_WithName_MustFillProperties(string name) =>
        new Account(name).UserName.Should().BeEquivalentTo(name);

    [Theory]
    [InlineData("name", "password")]
    public void Construct_WithNamePassword_MustFillProperties(string name, string password)
    {
        var actual = new Account(name, password);
        actual.UserName.Should().BeEquivalentTo(name);
        actual.PasswordHash.Should().BeEquivalentTo(password);
    }

    [Theory]
    [InlineData("name", "password")]
    public void Construct_WithNamePasswordRolesClaims_MustFillProperties(string name, string password)
    {
        var roles = Enumerable.Repeat<string>("name", 3).ToList();
        var claims = Enumerable.Repeat<AccountClaims>(AccountClaims.CLAIMS_USER_CAN_MODIFY, 3).ToList();

        var actual = new Account(name, password, roles, claims);
        actual.UserName.Should().BeEquivalentTo(name);
        actual.PasswordHash.Should().BeEquivalentTo(password);
        actual.Roles.SequenceEqual(roles).Should().BeTrue();
        actual.Claims.SequenceEqual(claims).Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "password", "Username is required")]
    [InlineData("", "password", "Username is required")]
    [InlineData(" ", "password", "Username is required")]

    [InlineData("name", null, "Password is required")]
    [InlineData("name", "", "Password is required")]
    [InlineData("name", " ", "Password is required")]
    public void Validate_Name_Password_MustHave_Message(string name, string password, string errorMessage)
    {
        var actual = new Account(name, password);
        actual
            .Invoking(x => x.Validate())
            .Should()
            .Throw<DomainValidationException>()
            .WithMessage(errorMessage);
    }
}
