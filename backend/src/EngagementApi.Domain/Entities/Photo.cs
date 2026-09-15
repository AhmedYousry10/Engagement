namespace EngagementApi.Domain.Entities;

public class Photo
{
    public int Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public DateTime UploadedAt { get; set; }
}
