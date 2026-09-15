using EngagementApi.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace EngagementApi.Infrastructure.Services;

public class FilePhotoStorageService : IPhotoStorageService
{
    private readonly string _uploadsRootPath;

    public FilePhotoStorageService(IOptions<StorageOptions> options)
    {
        _uploadsRootPath = options.Value.UploadsRootPath;
    }

    public async Task<string> SaveAsync(Stream content, string fileExtension, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_uploadsRootPath);

        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var fullPath = Path.Combine(_uploadsRootPath, fileName);

        await using (var fileStream = File.Create(fullPath))
        {
            await content.CopyToAsync(fileStream, cancellationToken);
        }

        return $"/uploads/{fileName}";
    }

    public void Delete(string urlPath)
    {
        var fileName = Path.GetFileName(urlPath);
        var fullPath = Path.Combine(_uploadsRootPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
