using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients
{
    public record ClientCreateRequestDto(string FirstName, string LastName, DateTime BirthDate)
    {
        public Client ToClient()
        {
            return new Client
            {
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = BirthDate
            };
        }
    }
}
