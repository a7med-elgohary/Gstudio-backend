using GStudio.Backend.Application.Common.Interfaces;
using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GStudio.Backend.Application.Services;

public class ServicesService : IServicesService
{
    private readonly IApplicationDbContext _db;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "services_list";

    public ServicesService(IApplicationDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<ServiceDto>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return await _db.Services
                .AsNoTracking()  // ✅ FIXED: Added AsNoTracking
                .OrderBy(s => s.Num)
                .Select(s => new ServiceDto(s.Id, s.Num, s.Title, s.Desc))
                .ToListAsync();
        }) ?? new();
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceRequest req)
    {
        // ✅ FIXED: Use MAX(Num) instead of COUNT(*) for better performance
        var maxNum = await _db.Services
            .AsNoTracking()
            .OrderByDescending(s => s.Num)
            .Select(s => s.Num)
            .FirstOrDefaultAsync() ?? "00";

        // Parse the numeric part and increment
        var nextNum = (int.Parse(maxNum) + 1).ToString("D2");

        var svc = new ServiceItem { Num = nextNum, Title = req.Title, Desc = req.Desc };
        _db.Services.Add(svc);
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return new ServiceDto(svc.Id, svc.Num, svc.Title, svc.Desc);
    }

    public async Task<bool> UpdateAsync(int id, UpdateServiceRequest req)
    {
        var svc = await _db.Services.FindAsync(id);
        if (svc is null) return false;
        svc.Title = req.Title; svc.Desc = req.Desc; svc.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var svc = await _db.Services.FindAsync(id);
        if (svc is null) return false;
        _db.Services.Remove(svc);
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return true;
    }
}
