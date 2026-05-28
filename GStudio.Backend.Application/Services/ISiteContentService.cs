using GStudio.Backend.Application.DTOs;

namespace GStudio.Backend.Application.Services;

public interface ISiteContentService
{
    Task<SiteContentDto> GetContentAsync();
    Task UpdateContentAsync(UpdateSiteContentRequest request);
}
