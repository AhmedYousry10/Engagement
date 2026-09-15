using EngagementApi.Domain.Entities;

namespace EngagementApi.Application.Common.Interfaces;

public interface IPasswordHasherService
{
    string Hash(AdminUser user, string password);
    bool Verify(AdminUser user, string hashedPassword, string providedPassword);
}
