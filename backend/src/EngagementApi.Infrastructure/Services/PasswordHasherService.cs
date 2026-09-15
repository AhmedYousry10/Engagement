using EngagementApi.Application.Common.Interfaces;
using EngagementApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EngagementApi.Infrastructure.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<AdminUser> _hasher = new();

    public string Hash(AdminUser user, string password) => _hasher.HashPassword(user, password);

    public bool Verify(AdminUser user, string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}
