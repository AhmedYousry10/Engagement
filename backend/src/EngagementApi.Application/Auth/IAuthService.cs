namespace EngagementApi.Application.Auth;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
}
