using GStudio.Backend.Application.Common.Interfaces;
using GStudio.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GStudio.Backend.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Work> Works => Set<Work>();
    public DbSet<ServiceItem> Services => Set<ServiceItem>();
    public DbSet<ClientLogo> ClientLogos => Set<ClientLogo>();
    public DbSet<SiteContent> SiteContents => Set<SiteContent>();
    public DbSet<PartnerCountry> PartnerCountries => Set<PartnerCountry>();
    public DbSet<ProjectRequest> ProjectRequests => Set<ProjectRequest>();
    public DbSet<CustomQuestion> CustomQuestions => Set<CustomQuestion>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Store Work.Gallery as JSON column
        modelBuilder.Entity<Work>(b =>
        {
            b.Property(w => w.Gallery)
             .HasColumnType("nvarchar(max)")
             .HasConversion(
                 v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                 v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new()
             );

            // Add indexes for frequently queried columns
            b.HasIndex(w => w.CreatedAt).IsDescending();
            b.HasIndex(w => w.Category);
        });

        // Add indexes on ProjectRequest for common queries
        modelBuilder.Entity<ProjectRequest>(b =>
        {
            b.HasIndex(r => r.Status);
            b.HasIndex(r => r.CreatedAt).IsDescending();
            b.HasIndex(r => r.Category);
        });

        // Add indexes on ClientLogo
        modelBuilder.Entity<ClientLogo>(b =>
        {
            b.HasIndex(l => l.CreatedAt).IsDescending();
        });

        // Add indexes on ServiceItem
        modelBuilder.Entity<ServiceItem>(b =>
        {
            b.HasIndex(s => s.Num);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
