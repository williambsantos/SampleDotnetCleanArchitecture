using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts
{
    public record AccountLoginResponseDto(string Token, DateTime ExpireDate)
    {
        public AccountLoginResponseDto(AccountToken userToken)
            : this(userToken.Token, userToken.ExpireDate)
        {
        }
    }
}
