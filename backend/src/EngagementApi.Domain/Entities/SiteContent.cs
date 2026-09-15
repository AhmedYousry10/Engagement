namespace EngagementApi.Domain.Entities;

public class SiteContent
{
    public int Id { get; set; }
    public string Name1 { get; set; } = string.Empty;
    public string Name2 { get; set; } = string.Empty;
    public string EventDateText { get; set; } = string.Empty;
    public DateOnly EventISODate { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationAddress { get; set; } = string.Empty;
    public string LocationMapUrl { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;

    public string ColorPrimary { get; set; } = "#9A4F44";
    public string ColorSecondary { get; set; } = "#8A9678";
    public string ColorBackground { get; set; } = "#FAF6F0";
    public string ColorText { get; set; } = "#2B2420";
}
