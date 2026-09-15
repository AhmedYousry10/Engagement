namespace EngagementApi.Application.Common.Interfaces;

public interface IPhotoStorageService
{
    /// <summary>Saves the file content and returns a URL path (e.g. "/uploads/abc.jpg") to store on the entity.</summary>
    Task<string> SaveAsync(Stream content, string fileExtension, CancellationToken cancellationToken = default);

    void Delete(string urlPath);
}
