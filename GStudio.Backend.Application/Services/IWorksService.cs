using GStudio.Backend.Application.DTOs;

namespace GStudio.Backend.Application.Services;

public interface IWorksService
{
    Task<List<WorkDto>> GetAllAsync();
    Task<WorkDto?> GetByIdAsync(int id);
    Task<WorkDto> CreateAsync(CreateWorkRequest req);
    Task<bool> UpdateAsync(int id, UpdateWorkRequest req);
    Task<bool> DeleteAsync(int id);
}
