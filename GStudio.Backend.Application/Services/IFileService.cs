namespace GStudio.Backend.Application.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(byte[] content, string fileName, string subFolder);
    void DeleteFile(string relativePath);
    bool IsManagedUpload(string? relativePath);
    void DeleteReplacedUpload(string? oldPath, string? newPath);
    void DeleteRemovedUploads(IEnumerable<string>? oldPaths, IEnumerable<string>? newPaths);
}
