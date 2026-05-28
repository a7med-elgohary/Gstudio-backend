namespace GStudio.Backend.Domain.Entities;

public class Work : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Services { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public List<string> Gallery { get; set; } = new();
}
