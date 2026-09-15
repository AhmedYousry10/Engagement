using EngagementApi.Domain.Entities;

namespace EngagementApi.Application.Common.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(AdminUser user);
}
