using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts
{
    public record AccountCreateResponseDto(string UserName)
    {
        public AccountCreateResponseDto(Account user) : this(user.UserName) { }
    }

}
