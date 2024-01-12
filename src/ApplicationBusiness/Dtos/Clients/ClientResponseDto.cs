using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients
{
    public record ClientResponseDto(long Id, string FirstName, string LastName, DateTime BirthDate)
    {
        public ClientResponseDto(Client client) : this(client.Id, client.FirstName, client.LastName, client.BirthDate)
        {
        }
    }

}
