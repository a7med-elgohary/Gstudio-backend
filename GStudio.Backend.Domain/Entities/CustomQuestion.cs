namespace GStudio.Backend.Domain.Entities;

public class CustomQuestion : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string Subtext { get; set; } = string.Empty;
}
