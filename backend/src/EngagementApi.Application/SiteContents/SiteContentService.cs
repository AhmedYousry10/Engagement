using EngagementApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngagementApi.Application.SiteContents;

public class SiteContentService : ISiteContentService
{
    private readonly IAppDbContext _db;

    public SiteContentService(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<SiteContentDto?> GetAsync(CancellationToken cancellationToken = default)
    {
        var content = await _db.SiteContents.FirstOrDefaultAsync(cancellationToken);
        return content is null ? null : ToDto(content);
    }

    public async Task<SiteContentDto?> UpdateAsync(SiteContentDto dto, CancellationToken cancellationToken = default)
    {
        var content = await _db.SiteContents.FirstOrDefaultAsync(cancellationToken);
        if (content is null)
        {
            return null;
        }

        content.Name1 = dto.Name1;
        content.Name2 = dto.Name2;
        content.EventDateText = dto.EventDateText;
        content.EventISODate = dto.EventISODate;
        content.LocationName = dto.LocationName;
        content.LocationAddress = dto.LocationAddress;
        content.LocationMapUrl = dto.LocationMapUrl;
        content.WhatsAppNumber = dto.WhatsAppNumber;
        content.ColorPrimary = dto.ColorPrimary;
        content.ColorSecondary = dto.ColorSecondary;
        content.ColorBackground = dto.ColorBackground;
        content.ColorText = dto.ColorText;

        await _db.SaveChangesAsync(cancellationToken);

        return ToDto(content);
    }

    private static SiteContentDto ToDto(Domain.Entities.SiteContent content) => new()
    {
        Name1 = content.Name1,
        Name2 = content.Name2,
        EventDateText = content.EventDateText,
        EventISODate = content.EventISODate,
        LocationName = content.LocationName,
        LocationAddress = content.LocationAddress,
        LocationMapUrl = content.LocationMapUrl,
        WhatsAppNumber = content.WhatsAppNumber,
        ColorPrimary = content.ColorPrimary,
        ColorSecondary = content.ColorSecondary,
        ColorBackground = content.ColorBackground,
        ColorText = content.ColorText
    };
}
