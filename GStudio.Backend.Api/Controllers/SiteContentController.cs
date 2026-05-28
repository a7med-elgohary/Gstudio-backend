using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GStudio.Backend.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SiteContentController : ControllerBase
{
    private readonly ISiteContentService _siteContentService;

    public SiteContentController(ISiteContentService siteContentService)
    {
        _siteContentService = siteContentService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<SiteContentDto>> GetContent()
    {
        return Ok(await _siteContentService.GetContentAsync());
    }

    [HttpPost]
    public async Task<ActionResult> UpdateContent(UpdateSiteContentRequest request)
    {
        await _siteContentService.UpdateContentAsync(request);
        return NoContent();
    }
}
