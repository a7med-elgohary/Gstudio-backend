using Microsoft.AspNetCore.Hosting;
using GStudio.Backend.Application.Services;

namespace GStudio.Backend.Infrastructure.Persistence;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(byte[] content, string fileName, string subFolder)
    {
        var wwwRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(wwwRootPath))
        {
            // Fallback for some environments where WebRootPath might be null initially
            wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadsFolder = Path.Combine(wwwRootPath, "uploads", subFolder);

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Ensure unique file name
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await File.WriteAllBytesAsync(filePath, content);

        // Return relative path for browser access
        return $"/uploads/{subFolder}/{uniqueFileName}";
    }

    public bool IsManagedUpload(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return false;
        var p = relativePath.Trim().Replace('\\', '/');
        return p.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)
            || p.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase);
    }

    public void DeleteReplacedUpload(string? oldPath, string? newPath)
    {
        if (!IsManagedUpload(oldPath)) return;

        var oldNorm = NormalizeUploadPath(oldPath!);
        var newNorm = string.IsNullOrWhiteSpace(newPath) ? "" : NormalizeUploadPath(newPath);

        if (!string.Equals(oldNorm, newNorm, StringComparison.OrdinalIgnoreCase))
            DeleteFile(oldPath!);
    }

    public void DeleteRemovedUploads(IEnumerable<string>? oldPaths, IEnumerable<string>? newPaths)
    {
        var retained = new HashSet<string>(
            (newPaths ?? []).Where(IsManagedUpload).Select(NormalizeUploadPath),
            StringComparer.OrdinalIgnoreCase);

        foreach (var old in oldPaths ?? [])
        {
            if (!IsManagedUpload(old)) continue;
            if (!retained.Contains(NormalizeUploadPath(old)))
                DeleteFile(old);
        }
    }

    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;

        var wwwRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(wwwRootPath))
            wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        // relativePath is e.g. "/uploads/images/guid_name.jpg"
        var normalizedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(wwwRootPath, normalizedPath);

        if (File.Exists(fullPath))
        {
            try { File.Delete(fullPath); } catch { /* ignore IO errors on shared hosting */ }
        }
    }

    private static string NormalizeUploadPath(string path)
        => path.Trim().Replace('\\', '/').TrimStart('/');
}
