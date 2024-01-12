using SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces;
using SampleDotnetCleanArchitecture.Domain.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Applications
{
    public class ClientApplication : IClientApplication
    {
        private readonly IClientService _clientService;

        public ClientApplication(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<ICollection<ClientResponseDto>> ListAsync()
        {
            ICollection<Client> clients = await _clientService.ListAsync();
            if (null == clients) return new List<ClientResponseDto>();
            return clients.Select(c => new ClientResponseDto(c)).ToList();
        }

        public async Task<ClientResponseDto?> GetByIdAsync(long id)
        {
            Client? client = await _clientService.GetByIdAsync(id);
            if (null == client) return null;
            return new ClientResponseDto(client);
        }

        public async Task<ClientCreateResponseDto> CreateAsync(ClientCreateRequestDto request, string? currentAccount)
        {
            var client = request.ToClient();
            client.CreatedBy = currentAccount;
            client.CreationDate = DateTime.Now;
            var clientCreated = await _clientService.CreateAsync(client);
            return new ClientCreateResponseDto(clientCreated);
        }

        public async Task UpdateAsync(long id, ClientUpdateRequestDto request, string? currentAccount)
        {
            var client = request.ToClient();
            client.LastModifiedBy = currentAccount;
            client.LastModifiedDate = DateTime.Now;
            await _clientService.UpdateAsync(id, client);
        }

        public async Task DeleteAsync(long id)
        {
            await _clientService.DeleteAsync(id);
        }
    }
}
