namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities
{
    public record AccountToken(string UserName, string Token, DateTime ExpireDate);
}
