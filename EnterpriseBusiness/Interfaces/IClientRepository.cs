using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces
{
    public interface IClientRepository
    {
        Task<ICollection<Client>> ListAsync();
        Task<Client?> GetByIdAsync(long id);
        Task<Client> CreateAsync(Client client);
        Task UpdateAsync(long id, Client client);
        Task DeleteAsync(long id);
    }
}
