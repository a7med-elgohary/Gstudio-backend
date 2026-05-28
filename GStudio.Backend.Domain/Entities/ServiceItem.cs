namespace GStudio.Backend.Domain.Entities;

public class ServiceItem : BaseEntity
{
    public string Num { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
}
