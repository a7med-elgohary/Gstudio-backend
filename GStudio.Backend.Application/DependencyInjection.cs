using GStudio.Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GStudio.Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWorksService, WorksService>();
        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<IClientLogosService, ClientLogosService>();
        services.AddScoped<ISiteContentService, SiteContentService>();
        
        return services;
    }
}
