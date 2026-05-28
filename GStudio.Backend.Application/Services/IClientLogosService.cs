using GStudio.Backend.Application.DTOs;

namespace GStudio.Backend.Application.Services;

public interface IClientLogosService
{
    Task<List<ClientLogoDto>> GetAllAsync();
    Task<ClientLogoDto> CreateAsync(CreateClientLogoRequest req);
    Task<bool> DeleteAsync(int id);
}
