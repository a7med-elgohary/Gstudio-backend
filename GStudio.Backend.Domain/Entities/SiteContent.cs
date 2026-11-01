namespace GStudio.Backend.Domain.Entities;

public class SiteContent : BaseEntity
{
    // Hero Section
    public string? HeroVideoUrl { get; set; }
    public string? HeroVideoUrlMobile { get; set; }
    public string? HeroImageUrl { get; set; }

    // About Section
    public string? AboutTagline { get; set; }
    public string? AboutTitleLine1 { get; set; }
    public string? AboutTitleLine2 { get; set; }
    public string? AboutTitleAccent { get; set; }
    public string? AboutDescription { get; set; }

    // Contact Section
    public string? ContactEmail { get; set; }
    public string? ContactWhatsapp { get; set; }
    public string? ContactLocation { get; set; }

    // Social Media Links
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? BehanceUrl { get; set; }
    public string? TikTokUrl { get; set; }

    // Statistics Section
    public string? Stat1Number { get; set; }
    public string? Stat1Label { get; set; }
    public string? Stat2Number { get; set; }
    public string? Stat2Label { get; set; }
    public string? Stat3Number { get; set; }
    public string? Stat3Label { get; set; }
    public string? Stat4Number { get; set; }
    public string? Stat4Label { get; set; }
}
