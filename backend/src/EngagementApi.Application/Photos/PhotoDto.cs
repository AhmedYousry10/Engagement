namespace EngagementApi.Application.Photos;

public class PhotoDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public DateTime UploadedAt { get; set; }
}
