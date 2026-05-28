using System.Linq;
using System.Threading.Tasks;
using GStudio.Backend.Application.DTOs;
using GStudio.Backend.Application.Interfaces;
using GStudio.Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GStudio.Backend.Application.Services;

namespace GStudio.Backend.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly ApplicationDbContext _db;
        private readonly IFileService _fileService;
        private readonly GStudio.Backend.Api.Services.RequestPdfService _pdfService;

        public RequestsController(IRequestService requestService, ApplicationDbContext db, IFileService fileService, GStudio.Backend.Api.Services.RequestPdfService pdfService)
        {
            _requestService = requestService;
            _db = db;
            _fileService = fileService;
            _pdfService = pdfService;
        }

        [AllowAnonymous]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitRequest([FromBody] ProjectRequestDto dto)
        {
            var result = await _requestService.SubmitProjectRequestAsync(dto);
            if (result != null)
                return Ok(new { Message = "Request submitted successfully." });

            return StatusCode(500, new { Message = "An error occurred." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            if (skip < 0) skip = 0;
            if (take < 1 || take > 500) take = 50;

            var reqs = await _db.ProjectRequests
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var list = reqs.Select(r => new {
                r.Id, r.ClientName, r.Phone, r.Company, r.Category,
                r.BrandName, r.Location, r.Tagline, r.NameMeaning,
                r.LogoName, r.DecisionMakers, r.IndustryTraits, r.Deadline,
                r.Vision, r.FoundingDate, r.LaunchDate, r.Scope,
                r.TargetAudience, r.Competitors, r.Status, r.CreatedAt,
                r.LogoTypes, r.Colors, r.CustomColor, r.SelectedTraits,
                r.LikedLogosReason, r.Dislikes, r.AdditionalInfo,
                Images = !string.IsNullOrEmpty(r.Images) 
                    ? r.Images.Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(img => img.Trim()).ToArray() 
                    : new string[0]
            });

            return Ok(new { data = list, skip, take });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var req = await _db.ProjectRequests.FindAsync(id);
            if (req == null) return NotFound();
            req.Status = dto.Status;
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var req = await _db.ProjectRequests.FindAsync(id);
            if (req == null) return NotFound();

            // Cleanup images
            if (!string.IsNullOrEmpty(req.Images))
            {
                var imageList = req.Images.Split(',', StringSplitOptions.RemoveEmptyEntries);
                _ = Task.Run(() =>
                {
                    foreach (var img in imageList)
                    {
                        _fileService.DeleteFile(img.Trim());
                    }
                });
            }

            _db.ProjectRequests.Remove(req);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectRequestDto dto)
        {
            var req = await _db.ProjectRequests.FindAsync(id);
            if (req == null) return NotFound();

            // Update fields from DTO (manual mapping for simplicity)
            req.ClientName = dto.ClientName;
            req.Phone = dto.Phone;
            req.Company = dto.Company;
            req.Location = dto.Location;
            req.Tagline = dto.Tagline;
            req.Category = dto.Category;
            req.BrandName = dto.BrandName;
            req.NameMeaning = dto.NameMeaning;
            req.LogoName = dto.LogoName;
            req.DecisionMakers = dto.DecisionMakers;
            req.IndustryTraits = dto.IndustryTraits;
            req.Deadline = dto.Deadline;
            req.Vision = dto.Vision;
            req.FoundingDate = dto.FoundingDate;
            req.LaunchDate = dto.LaunchDate;
            req.Scope = dto.Scope;
            req.TargetAudience = dto.TargetAudience;
            req.Competitors = dto.Competitors;
            req.AdditionalInfo = dto.AdditionalInfo;
            req.LikedLogosReason = dto.LikedLogosReason;
            req.Dislikes = dto.Dislikes;
            req.CustomColor = dto.CustomColor;

            // Handle arrays/lists (stored as comma strings)
            req.LogoTypes = dto.LogoTypes != null ? string.Join(",", dto.LogoTypes) : "";
            req.Colors = dto.Colors != null ? string.Join(",", dto.Colors) : "";
            req.SelectedTraits = dto.SelectedTraits != null ? string.Join(",", dto.SelectedTraits) : "";

            var oldImages = string.IsNullOrEmpty(req.Images)
                ? Array.Empty<string>()
                : req.Images.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
            var newImages = dto.Images ?? new List<string>();
            _fileService.DeleteRemovedUploads(oldImages, newImages);
            req.Images = newImages.Count > 0 ? string.Join(",", newImages) : "";

            await _db.SaveChangesAsync();
            return Ok(new { Message = "Request updated successfully" });
        }

        [HttpGet("{id}/pdf")]
        [AllowAnonymous] // Allow viewing PDF without auth for easy download
        public async Task<IActionResult> GetPdf(int id)
        {
            var req = await _db.ProjectRequests.FindAsync(id);
            if (req == null) return NotFound();

            var pdfBytes = _pdfService.GenerateRequestPdf(req);
            return File(pdfBytes, "application/pdf", $"GStudio_Request_{id}.pdf");
        }
    }

    public class StatusUpdateDto
    {
        public string Status { get; set; } = "New";
    }
}
