using GStudio.Backend.Application.Common.Interfaces;
using GStudio.Backend.Application.Services;
using GStudio.Backend.Application.Interfaces;
using GStudio.Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GStudio.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b
                    .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                    .CommandTimeout(30)
                    .EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: System.TimeSpan.FromSeconds(5), errorNumbersToAdd: null)
            ),
            ServiceLifetime.Scoped
        );

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IRequestService, GStudio.Backend.Infrastructure.Services.RequestService>();
        return services;
    }
}
