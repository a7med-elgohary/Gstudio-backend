using GStudio.Backend.Domain.Entities;
using GStudio.Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace GStudio.Backend.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PartnerCountriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "partner_countries_list";

    public PartnerCountriesController(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartnerCountry>>> Get()
    {
        var list = await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return await _context.PartnerCountries.AsNoTracking().ToListAsync();
        });
        return Ok(list ?? new List<PartnerCountry>());
    }

    [HttpPost]
    public async Task<ActionResult<PartnerCountry>> Create(PartnerCountry country)
    {
        _context.PartnerCountries.Add(country);
        await _context.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return country;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var country = await _context.PartnerCountries.FindAsync(id);
        if (country == null) return NotFound();

        _context.PartnerCountries.Remove(country);
        await _context.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return NoContent();
    }
}
