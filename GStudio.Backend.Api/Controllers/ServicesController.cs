using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GStudio.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServicesService _svc;
    public ServicesController(IServicesService svc) => _svc = svc;

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest req)
        => Ok(await _svc.CreateAsync(req));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceRequest req)
        => await _svc.UpdateAsync(id, req) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}
