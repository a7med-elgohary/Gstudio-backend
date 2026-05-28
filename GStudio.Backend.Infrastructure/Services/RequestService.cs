using System;
using System.Linq;
using System.Threading.Tasks;
using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Application.Interfaces;
using GStudio.Backend.Domain.Entities;
using GStudio.Backend.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace GStudio.Backend.Infrastructure.Services
{
    public class RequestService : IRequestService
    {
        private readonly ILogger<RequestService> _logger;
        private readonly ApplicationDbContext _db;

        public RequestService(ILogger<RequestService> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<string?> SubmitProjectRequestAsync(ProjectRequestDto dto)
        {
            try
            {
                var entity = new ProjectRequest
                {
                    ClientName      = dto.ClientName      ?? "",
                    Phone           = dto.Phone           ?? "",
                    Company         = dto.Company         ?? "",
                    Location        = dto.Location        ?? "",
                    Tagline         = dto.Tagline         ?? "",
                    Category        = dto.Category        ?? "",
                    BrandName       = dto.BrandName       ?? "",
                    NameMeaning     = dto.NameMeaning     ?? "",
                    LogoName        = dto.LogoName        ?? "",
                    DecisionMakers  = dto.DecisionMakers  ?? "",
                    IndustryTraits  = dto.IndustryTraits  ?? "",
                    Deadline        = dto.Deadline        ?? "",
                    Vision          = dto.Vision          ?? "",
                    FoundingDate    = dto.FoundingDate    ?? "",
                    LaunchDate      = dto.LaunchDate      ?? "",
                    Scope           = dto.Scope           ?? "",
                    TargetAudience  = dto.TargetAudience  ?? "",
                    Competitors     = dto.Competitors     ?? "",
                    LogoTypes       = dto.LogoTypes != null ? string.Join(", ", dto.LogoTypes) : "",
                    Colors          = dto.Colors    != null ? string.Join(", ", dto.Colors)    : "",
                    CustomColor     = dto.CustomColor     ?? "",
                    SelectedTraits  = dto.SelectedTraits != null ? string.Join(", ", dto.SelectedTraits) : "",
                    LikedLogosReason= dto.LikedLogosReason ?? "",
                    Dislikes        = dto.Dislikes        ?? "",
                    AdditionalInfo  = dto.AdditionalInfo  ?? "",
                    Images          = dto.Images != null 
                                        ? string.Join(",", dto.Images.Where(i => !string.IsNullOrWhiteSpace(i))) 
                                        : "",
                    Status          = "New",
                    CreatedAt       = DateTime.UtcNow
                };

                _db.ProjectRequests.Add(entity);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Project request saved — ID: {Id}, Client: {Client}", entity.Id, entity.ClientName);
                return "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving project request.");
                return null;
            }
        }
    }
}
