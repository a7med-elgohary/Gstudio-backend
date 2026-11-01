using GStudio.Backend.Application.Common.Interfaces;
using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GStudio.Backend.Application.Services;

public class SiteContentService : ISiteContentService
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "site_content";

    public SiteContentService(IApplicationDbContext context, IFileService fileService, IMemoryCache cache)
    {
        _context = context;
        _fileService = fileService;
        _cache = cache;
    }

    public async Task<SiteContentDto> GetContentAsync()
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            var content = await _context.SiteContents
                .AsNoTracking()  // ✅ Already present but confirmed
                .FirstOrDefaultAsync();
            if (content == null) return GetDefaultContent();
            return MapToDto(content);
        }) ?? GetDefaultContent();
    }

    public async Task UpdateContentAsync(UpdateSiteContentRequest request)
    {
        var content = await _context.SiteContents.FirstOrDefaultAsync();

        if (content == null)
        {
            content = new SiteContent();
            _context.SiteContents.Add(content);
        }
        else
        {
            // ✅ FIXED: Delete old files asynchronously
            _ = Task.Run(() =>
            {
                _fileService.DeleteReplacedUpload(content.HeroVideoUrl, request.HeroVideoUrl);
                _fileService.DeleteReplacedUpload(content.HeroVideoUrlMobile, request.HeroVideoUrlMobile);
                _fileService.DeleteReplacedUpload(content.HeroImageUrl, request.HeroImageUrl);
            });
        }

        content.HeroVideoUrl = request.HeroVideoUrl;
        content.HeroVideoUrlMobile = request.HeroVideoUrlMobile;
        content.HeroImageUrl = request.HeroImageUrl;
        content.AboutTagline = request.AboutTagline;
        content.AboutTitleLine1 = request.AboutTitleLine1;
        content.AboutTitleLine2 = request.AboutTitleLine2;
        content.AboutTitleAccent = request.AboutTitleAccent;
        content.AboutDescription = request.AboutDescription;
        content.ContactEmail = request.ContactEmail;
        content.ContactWhatsapp = request.ContactWhatsapp;
        content.ContactLocation = request.ContactLocation;
        content.FacebookUrl = request.FacebookUrl;
        content.InstagramUrl = request.InstagramUrl;
        content.LinkedInUrl = request.LinkedInUrl;
        content.BehanceUrl = request.BehanceUrl;
        content.TikTokUrl = request.TikTokUrl;
        content.Stat1Number = request.Stat1Number;
        content.Stat1Label = request.Stat1Label;
        content.Stat2Number = request.Stat2Number;
        content.Stat2Label = request.Stat2Label;
        content.Stat3Number = request.Stat3Number;
        content.Stat3Label = request.Stat3Label;
        content.Stat4Number = request.Stat4Number;
        content.Stat4Label = request.Stat4Label;

        await _context.SaveChangesAsync();
        
        _cache.Remove(CacheKey);
    }

    private SiteContentDto MapToDto(SiteContent c) => new SiteContentDto(
        c.HeroVideoUrl ?? "", c.HeroVideoUrlMobile ?? "", c.HeroImageUrl ?? "",
        c.AboutTagline ?? "", c.AboutTitleLine1 ?? "", c.AboutTitleLine2 ?? "", c.AboutTitleAccent ?? "", c.AboutDescription ?? "",
        c.ContactEmail ?? "", c.ContactWhatsapp ?? "", c.ContactLocation ?? "",
        c.FacebookUrl ?? "", c.InstagramUrl ?? "", c.LinkedInUrl ?? "", c.BehanceUrl ?? "", c.TikTokUrl ?? "",
        c.Stat1Number ?? "", c.Stat1Label ?? "",
        c.Stat2Number ?? "", c.Stat2Label ?? "",
        c.Stat3Number ?? "", c.Stat3Label ?? "",
        c.Stat4Number ?? "", c.Stat4Label ?? "");

    private SiteContentDto GetDefaultContent() => new SiteContentDto(
        "", "", "", // Hero
        "", "", "", "", "", // About
        "", "", "", // Contact
        "", "", "", "", "", // Social
        "", "", "", "", "", "", "", ""); // Stats
}
