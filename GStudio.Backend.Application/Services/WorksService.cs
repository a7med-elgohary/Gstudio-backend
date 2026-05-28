using GStudio.Backend.Application.Common.Interfaces;
using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GStudio.Backend.Application.Services;

public class WorksService : IWorksService
{
    private readonly IApplicationDbContext _db;
    private readonly IFileService _fileService;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "works_list";

    public WorksService(IApplicationDbContext db, IFileService fileService, IMemoryCache cache)
    {
        _db = db;
        _fileService = fileService;
        _cache = cache;
    }

    public async Task<List<WorkDto>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return await _db.Works
                .AsNoTracking()  // ✅ FIXED: Added AsNoTracking for read-only query
                .OrderByDescending(w => w.CreatedAt)
                .Select(w => new WorkDto(w.Id, w.Title, w.Client, w.Category,
                    w.Year, w.Services, w.Description, w.Image, w.Gallery, w.CreatedAt))
                .ToListAsync();
        }) ?? new();
    }

    public async Task<WorkDto?> GetByIdAsync(int id)
    {
        var w = await _db.Works
            .AsNoTracking()  // ✅ FIXED: Added AsNoTracking
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (w is null) return null;
        return new WorkDto(w.Id, w.Title, w.Client, w.Category,
            w.Year, w.Services, w.Description, w.Image, w.Gallery, w.CreatedAt);
    }

    public async Task<WorkDto> CreateAsync(CreateWorkRequest req)
    {
        var work = new Work
        {
            Title = req.Title, Client = req.Client, Category = req.Category,
            Year = req.Year, Services = req.Services, Description = req.Description,
            Image = req.Image, Gallery = req.Gallery ?? new()
        };
        _db.Works.Add(work);
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return new WorkDto(work.Id, work.Title, work.Client, work.Category,
            work.Year, work.Services, work.Description, work.Image, work.Gallery, work.CreatedAt);
    }

    public async Task<bool> UpdateAsync(int id, UpdateWorkRequest req)
    {
        var work = await _db.Works.FindAsync(id);
        if (work is null) return false;

        _fileService.DeleteReplacedUpload(work.Image, req.Image);
        _fileService.DeleteRemovedUploads(work.Gallery, req.Gallery ?? new());

        work.Title = req.Title; work.Client = req.Client; work.Category = req.Category;
        work.Year = req.Year; work.Services = req.Services; work.Description = req.Description;
        work.Image = req.Image; work.Gallery = req.Gallery ?? new();
        work.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var work = await _db.Works.FindAsync(id);
        if (work is null) return false;

        // Delete images from disk asynchronously in background
        _ = Task.Run(() =>
        {
            _fileService.DeleteFile(work.Image);
            if (work.Gallery != null)
            {
                foreach (var img in work.Gallery)
                {
                    _fileService.DeleteFile(img);
                }
            }
        });

        _db.Works.Remove(work);
        await _db.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
        return true;
    }
}
