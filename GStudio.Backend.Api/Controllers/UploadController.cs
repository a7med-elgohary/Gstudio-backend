using GStudio.Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;

namespace GStudio.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _environment;

    public UploadController(IFileService fileService, IWebHostEnvironment environment)
    {
        _fileService = fileService;
        _environment = environment;
    }

    [HttpGet("download-backup")]
    public IActionResult DownloadBackup()
    {
        var wwwRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(wwwRootPath))
        {
            wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadsPath = Path.Combine(wwwRootPath, "uploads");

        if (!Directory.Exists(uploadsPath))
        {
            return NotFound("Uploads folder does not exist.");
        }

        // Save the temp zip in wwwroot (outside of uploads folder) to ensure write permissions on shared hosting
        var tempZipPath = Path.Combine(wwwRootPath, $"uploads_backup_{Guid.NewGuid()}.zip");

        try
        {
            // Create zip archive from directory
            ZipFile.CreateFromDirectory(uploadsPath, tempZipPath);

            // Use FileOptions.DeleteOnClose to automatically delete the file once streaming is complete
            var fileStream = new FileStream(tempZipPath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);

            return File(fileStream, "application/zip", "uploads_backup.zip");
        }
        catch (Exception ex)
        {
            // Ensure temp file is cleaned up in case of failure before stream opening
            if (System.IO.File.Exists(tempZipPath))
            {
                try { System.IO.File.Delete(tempZipPath); } catch { }
            }
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("debug-uploads")]
    public IActionResult DebugUploads()
    {
        var wwwRootPath = _environment.WebRootPath;
        var fallbackPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var resolvedPath = wwwRootPath ?? fallbackPath;
        var uploadsPath = Path.Combine(resolvedPath, "uploads");

        var filesList = new List<object>();
        if (Directory.Exists(uploadsPath))
        {
            var files = Directory.GetFiles(uploadsPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                filesList.Add(new
                {
                    RelativePath = Path.GetRelativePath(resolvedPath, file),
                    Size = fileInfo.Length,
                    CreatedAt = fileInfo.CreationTimeUtc
                });
            }
        }

        var info = new
        {
            WebRootPath = wwwRootPath,
            CurrentDirectory = Directory.GetCurrentDirectory(),
            UploadsPath = uploadsPath,
            UploadsFolderExists = Directory.Exists(uploadsPath),
            FilesCount = filesList.Count,
            Files = filesList
        };

        return Ok(info);
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IEnumerable<IFormFile> files, [FromQuery] string type = "images")
    {
        if (files == null || !files.Any())
            return BadRequest("No files uploaded.");

        var urls = new List<string>();

        // Clean folder name to prevent directory traversal
        var (folder, prefix) = type.ToLower() switch
        {
            "video"      => ("video", ""),
            "videos"     => ("video", ""),
            "icon"       => ("assets", "icon_"),
            "icons"      => ("assets", "icon_"),
            "liked"      => ("photos", "ref_"),
            "letstalk"   => ("photos", "ref_"),
            "references" => ("photos", "ref_"),
            "support"    => ("photos", "sup_"),
            "images"     => ("photos", "sup_"),
            "projects"   => ("projects", "proj_"),
            _            => ("photos", "file_")
        };

        foreach (var file in files)
        {
            if (file.Length == 0) continue;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var fileName = prefix + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = await _fileService.SaveFileAsync(fileBytes, fileName, folder);
            urls.Add(path);
        }

        // Return first URL for backward compatibility if needed, or the whole list
        // Since we are moving to multi-upload, returning the list is better.
        // If the frontend expects a single URL (for single uploader), it can take urls[0].
        return Ok(new { url = urls.FirstOrDefault(), urls = urls });
    }
}
