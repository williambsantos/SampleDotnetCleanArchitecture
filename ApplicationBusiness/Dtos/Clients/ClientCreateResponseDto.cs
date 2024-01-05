using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients
{
    public record ClientCreateResponseDto(long Id, string FirstName, string LastName, DateTime BirthDate)
    {
        public ClientCreateResponseDto(Client client) : this(client.Id, client.FirstName, client.LastName, client.BirthDate) { }
    }

}
