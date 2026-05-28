using System.Threading.Tasks;
using GStudio.Backend.Application.DTOs;

namespace GStudio.Backend.Application.Interfaces
{
    public interface IRequestService
    {
        Task<string?> SubmitProjectRequestAsync(ProjectRequestDto request);
    }
}
