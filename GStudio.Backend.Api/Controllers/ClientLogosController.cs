using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GStudio.Backend.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientLogosController : ControllerBase
{
    private readonly IClientLogosService _svc;
    public ClientLogosController(IClientLogosService svc) => _svc = svc;

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientLogoRequest req)
        => Ok(await _svc.CreateAsync(req));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}
