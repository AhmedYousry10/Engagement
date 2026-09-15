using EngagementApi.Application.Common.Interfaces;
using EngagementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngagementApi.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SiteContent> SiteContents => Set<SiteContent>();
    public DbSet<DetailCard> DetailCards => Set<DetailCard>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
