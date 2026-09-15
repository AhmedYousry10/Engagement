namespace EngagementApi.Application.SiteContents;

public interface ISiteContentService
{
    Task<SiteContentDto?> GetAsync(CancellationToken cancellationToken = default);
    Task<SiteContentDto?> UpdateAsync(SiteContentDto dto, CancellationToken cancellationToken = default);
}
