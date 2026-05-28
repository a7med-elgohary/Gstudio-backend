using GStudio.Backend.Application.Common.Interfaces;
using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GStudio.Backend.Application.Services;

public class ClientLogosService : IClientLogosService
{
    private readonly IApplicationDbContext _db;
    private readonly IFileService _fileService;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "client_logos_list";

    public ClientLogosService(IApplicationDbContext db, IFileService fileService, IMemoryCache cache)
    {
        _db = db;
        _fileService = fileService;
        _cache = cache;
    }

    public async Task<List<ClientLogoDto>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return await _db.ClientLogos
                .AsNoTracking()  // ✅ FIXED: Added AsNoTracking
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new ClientLogoDto(l.Id, l.ImageUrl))
                .ToListAsync();
        }) ?? new();
    }

    public async Task<ClientLogoDto> CreateAsync(CreateClientLogoRequest req)
    {
        var logo = new ClientLogo { ImageUrl = req.ImageUrl };
        _db.ClientLogos.Add(logo);
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return new ClientLogoDto(logo.Id, logo.ImageUrl);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var logo = await _db.ClientLogos.FindAsync(id);
        if (logo is null) return false;

        // ✅ FIXED: Delete files asynchronously in background
        _ = Task.Run(() => _fileService.DeleteFile(logo.ImageUrl));

        _db.ClientLogos.Remove(logo);
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return true;
    }
}
