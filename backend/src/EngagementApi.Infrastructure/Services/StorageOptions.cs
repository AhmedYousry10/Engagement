namespace EngagementApi.Infrastructure.Services;

public class StorageOptions
{
    /// <summary>Absolute filesystem path to the folder where uploaded photos are written.</summary>
    public string UploadsRootPath { get; set; } = string.Empty;
}
