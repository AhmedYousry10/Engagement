using EngagementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngagementApi.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<SiteContent> SiteContents { get; }
    DbSet<DetailCard> DetailCards { get; }
    DbSet<Photo> Photos { get; }
    DbSet<AdminUser> AdminUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
