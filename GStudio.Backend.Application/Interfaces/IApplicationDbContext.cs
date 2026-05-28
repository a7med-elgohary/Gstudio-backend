using GStudio.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GStudio.Backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Work> Works { get; }
    DbSet<ServiceItem> Services { get; }
    DbSet<ClientLogo> ClientLogos { get; }
    DbSet<SiteContent> SiteContents { get; }
    DbSet<CustomQuestion> CustomQuestions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
