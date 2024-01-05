using SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces
{
    public interface IClientApplication
    {
        Task<ICollection<ClientResponseDto>> ListAsync();
        Task<ClientResponseDto?> GetByIdAsync(long id);
        Task<ClientCreateResponseDto> CreateAsync(ClientCreateRequestDto request, string? currentAccount);
        Task UpdateAsync(long id, ClientUpdateRequestDto request, string? currentAccount);
        Task DeleteAsync(long id);
    }
}
