using EngagementApi.Application.Common.Exceptions;
using EngagementApi.Application.Common.Interfaces;
using EngagementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngagementApi.Application.Photos;

public class PhotosService : IPhotosService
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    private readonly IAppDbContext _db;
    private readonly IPhotoStorageService _storage;

    public PhotosService(IAppDbContext db, IPhotoStorageService storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<List<PhotoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Photos
            .OrderBy(p => p.SortOrder)
            .Select(p => ToDto(p))
            .ToListAsync(cancellationToken);
    }

    public async Task<PhotoDto> UploadAsync(Stream content, string fileName, long length, CancellationToken cancellationToken = default)
    {
        if (length <= 0)
        {
            throw new PhotoValidationException("No file uploaded.");
        }

        if (length > MaxFileSizeBytes)
        {
            throw new PhotoValidationException("File exceeds the 10 MB limit.");
        }

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new PhotoValidationException("Unsupported file type.");
        }

        var urlPath = await _storage.SaveAsync(content, extension, cancellationToken);

        var maxSortOrder = await _db.Photos.Select(p => (int?)p.SortOrder).MaxAsync(cancellationToken) ?? 0;

        var photo = new Photo
        {
            FilePath = urlPath,
            SortOrder = maxSortOrder + 1,
            UploadedAt = DateTime.UtcNow
        };

        _db.Photos.Add(photo);
        await _db.SaveChangesAsync(cancellationToken);

        return ToDto(photo);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var photo = await _db.Photos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (photo is null)
        {
            return false;
        }

        _storage.Delete(photo.FilePath);

        _db.Photos.Remove(photo);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static PhotoDto ToDto(Photo photo) => new()
    {
        Id = photo.Id,
        Url = photo.FilePath,
        SortOrder = photo.SortOrder,
        UploadedAt = photo.UploadedAt
    };
}
