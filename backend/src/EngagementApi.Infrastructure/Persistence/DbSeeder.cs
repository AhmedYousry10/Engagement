using EngagementApi.Application.Common.Interfaces;
using EngagementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EngagementApi.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IPasswordHasherService passwordHasher, IConfiguration configuration)
    {
        await db.Database.MigrateAsync();

        if (!await db.SiteContents.AnyAsync())
        {
            db.SiteContents.Add(new SiteContent
            {
                Name1 = "Partner 1",
                Name2 = "Partner 2",
                EventDateText = "Friday, December 4, 2026",
                EventISODate = new DateOnly(2026, 12, 4),
                LocationName = string.Empty,
                LocationAddress = string.Empty,
                LocationMapUrl = string.Empty,
                WhatsAppNumber = string.Empty
            });
        }

        if (!await db.AdminUsers.AnyAsync())
        {
            var seedUsername = configuration["SeedAdmin:Username"] ?? "admin";
            var seedPassword = configuration["SeedAdmin:Password"] ?? "ChangeMe123!";

            var admin = new AdminUser { Username = seedUsername };
            admin.PasswordHash = passwordHasher.Hash(admin, seedPassword);

            db.AdminUsers.Add(admin);
        }

        await db.SaveChangesAsync();
    }
}
