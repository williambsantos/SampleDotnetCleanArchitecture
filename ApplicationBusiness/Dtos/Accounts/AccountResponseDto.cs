using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts
{
    public record AccountResponseDto(string UserName, ICollection<string> Roles, ICollection<AccountClaims> Claims)
    {
        public AccountResponseDto(Account user) : this(user.UserName, user.Roles, user.Claims) { }
    }

}
