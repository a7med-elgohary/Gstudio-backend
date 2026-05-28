using GStudio.Backend.Application;
using GStudio.Backend.Infrastructure;
using GStudio.Backend.Infrastructure.Persistence;
using GStudio.Backend.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GStudio.Backend.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Services ──────────────────────────────────────────
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddResponseCaching();
            builder.Services.AddScoped<GStudio.Backend.Api.Services.RequestPdfService>();
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // ── JWT Authentication ─────────────────────────────────
            var jwtKey = builder.Configuration["Jwt:Key"]!;
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = builder.Configuration["Jwt:Issuer"],
                        ValidAudience            = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew                = TimeSpan.Zero
                    };
                });

            // Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "G.Studio API", Version = "v1" });
            });

            // CORS — allow React dev server
            builder.Services.AddCors(o => o.AddPolicy("AllowFrontend", p =>
                p.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:3001",
                    "http://localhost:5173",
                    "https://gstudio.eg.com",
                    "https://gstudio-art.com",
                    "https://gstudio.runasp.net"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetPreflightMaxAge(TimeSpan.FromHours(24))
            ));

            // ── Build ──────────────────────────────────────────────
            var app = builder.Build();
            app.UseResponseCompression();

            // ── Database Initialization ────────────────────────────
            /*
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    await DbInitializer.SeedAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            */

            // ── Pipeline ───────────────────────────────────────────
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "G.Studio API v1"));
            }

            app.UseCors("AllowFrontend");
            app.UseResponseCaching();
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            provider.Mappings[".webm"] = "video/webm";
            provider.Mappings[".webp"] = "image/webp";

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider,
                OnPrepareResponse = ctx =>
                {
                    // Allow CORS for static files
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

                    var path = ctx.Context.Request.Path.Value ?? "";

                    // robots.txt and sitemap.xml must NOT be cached long-term
                    // so crawlers and Lighthouse always get the latest version
                    if (path.EndsWith("robots.txt", StringComparison.OrdinalIgnoreCase) ||
                        path.EndsWith("sitemap.xml", StringComparison.OrdinalIgnoreCase))
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
                    }
                    else
                    {
                        // Cache images/videos for 1 year with 'public' for CDNs and Shared Cache
                        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
                    }
                },
                ServeUnknownFileTypes = true
            });
            
            // This is the important part for videos!
            // .NET Core 6+ supports range requests by default, 
            // but let's make sure it's enabled properly.
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Browsers request /favicon.ico by default; serve PNG logo (no 404)
            app.MapGet("/favicon.ico", (IWebHostEnvironment env) =>
            {
                var root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var path = Path.Combine(root, "favicon.ico");
                if (!File.Exists(path))
                    path = Path.Combine(root, "favicon-32.png");
                return File.Exists(path) ? Results.File(path, "image/png") : Results.NotFound();
            });
            
            // Map React SPA - important for production
            app.MapFallbackToFile("index.html");

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllText("startup_error.txt", ex.ToString());
            throw;
        }
    }
}
