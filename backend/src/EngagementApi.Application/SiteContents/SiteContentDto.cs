namespace EngagementApi.Application.SiteContents;

public class SiteContentDto
{
    public string Name1 { get; set; } = string.Empty;
    public string Name2 { get; set; } = string.Empty;
    public string EventDateText { get; set; } = string.Empty;
    public DateOnly EventISODate { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationAddress { get; set; } = string.Empty;
    public string LocationMapUrl { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string ColorPrimary { get; set; } = string.Empty;
    public string ColorSecondary { get; set; } = string.Empty;
    public string ColorBackground { get; set; } = string.Empty;
    public string ColorText { get; set; } = string.Empty;
}
