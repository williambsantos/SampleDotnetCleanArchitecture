using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts
{
    public record AccountCreateRequestDto(string Username, string Password, ICollection<string>? Roles, ICollection<AccountClaims>? Claims);
}
