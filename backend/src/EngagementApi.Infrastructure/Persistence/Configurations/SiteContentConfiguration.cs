using EngagementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngagementApi.Infrastructure.Persistence.Configurations;

public class SiteContentConfiguration : IEntityTypeConfiguration<SiteContent>
{
    public void Configure(EntityTypeBuilder<SiteContent> builder)
    {
        builder.Property(c => c.Name1).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Name2).HasMaxLength(200).IsRequired();
        builder.Property(c => c.EventDateText).HasMaxLength(200).IsRequired();
        builder.Property(c => c.LocationName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.LocationAddress).HasMaxLength(400).IsRequired();
        builder.Property(c => c.LocationMapUrl).HasMaxLength(1000).IsRequired();
        builder.Property(c => c.WhatsAppNumber).HasMaxLength(30).IsRequired();
        builder.Property(c => c.ColorPrimary).HasMaxLength(9).IsRequired();
        builder.Property(c => c.ColorSecondary).HasMaxLength(9).IsRequired();
        builder.Property(c => c.ColorBackground).HasMaxLength(9).IsRequired();
        builder.Property(c => c.ColorText).HasMaxLength(9).IsRequired();
    }
}
