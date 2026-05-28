using GStudio.Backend.Application.DTOs;

namespace GStudio.Backend.Application.Services;

public interface IServicesService
{
    Task<List<ServiceDto>> GetAllAsync();
    Task<ServiceDto> CreateAsync(CreateServiceRequest req);
    Task<bool> UpdateAsync(int id, UpdateServiceRequest req);
    Task<bool> DeleteAsync(int id);
}
