using SampleDotnetCleanArchitecture.Domain.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> CreateAsync(Client client)
        {
            if (client == null)
                throw new DomainValidationException("client required");

            client.Validate();

            return await _clientRepository.CreateAsync(client);
        }

        public async Task DeleteAsync(long id) => await _clientRepository.DeleteAsync(id);

        public async Task<Client?> GetByIdAsync(long id) => await _clientRepository.GetByIdAsync(id);

        public async Task<ICollection<Client>> ListAsync() => await _clientRepository.ListAsync();

        public async Task UpdateAsync(long id, Client client)
        {
            if (client == null)
                throw new DomainValidationException("client required");

            client.Validate();

            await _clientRepository.UpdateAsync(id, client);
        }
    }
}
