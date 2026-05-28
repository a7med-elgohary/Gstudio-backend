namespace GStudio.Backend.Domain.Entities;

public class PartnerCountry : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g., "us", "eg" for flag icons
}
