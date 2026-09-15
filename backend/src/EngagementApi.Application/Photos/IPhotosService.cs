namespace EngagementApi.Application.Photos;

public interface IPhotosService
{
    Task<List<PhotoDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Validates and stores an uploaded photo. Throws PhotoValidationException on invalid input.</summary>
    Task<PhotoDto> UploadAsync(Stream content, string fileName, long length, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
