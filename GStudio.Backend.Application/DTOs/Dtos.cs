namespace GStudio.Backend.Application.DTOs;

// ── Works ──────────────────────────────────────────
public record WorkDto(int Id, string Title, string Client, string Category,
    string Year, string Services, string Description, string Image, List<string> Gallery, DateTime CreatedAt);

public record CreateWorkRequest(string Title, string Client, string Category,
    string Year, string Services, string Description, string Image, List<string> Gallery);

public record UpdateWorkRequest(string Title, string Client, string Category,
    string Year, string Services, string Description, string Image, List<string> Gallery);

// ── Services ───────────────────────────────────────
public record ServiceDto(int Id, string Num, string Title, string Desc);
public record CreateServiceRequest(string Title, string Desc);
public record UpdateServiceRequest(string Title, string Desc);

// ── Client Logos ───────────────────────────────────
public record ClientLogoDto(int Id, string ImageUrl);
public record CreateClientLogoRequest(string ImageUrl);

// ── Site Content ───────────────────────────────────
public record SiteContentDto(
    string? HeroVideoUrl, string? HeroVideoUrlMobile, string? HeroImageUrl,
    string? AboutTagline, string? AboutTitleLine1, string? AboutTitleLine2, string? AboutTitleAccent, string? AboutDescription,
    string? ContactEmail, string? ContactWhatsapp, string? ContactLocation,
    string? Stat1Number, string? Stat1Label,
    string? Stat2Number, string? Stat2Label,
    string? Stat3Number, string? Stat3Label,
    string? Stat4Number, string? Stat4Label);

public record UpdateSiteContentRequest(
    string? HeroVideoUrl, string? HeroVideoUrlMobile, string? HeroImageUrl,
    string? AboutTagline, string? AboutTitleLine1, string? AboutTitleLine2, string? AboutTitleAccent, string? AboutDescription,
    string? ContactEmail, string? ContactWhatsapp, string? ContactLocation,
    string? Stat1Number, string? Stat1Label,
    string? Stat2Number, string? Stat2Label,
    string? Stat3Number, string? Stat3Label,
    string? Stat4Number, string? Stat4Label);

