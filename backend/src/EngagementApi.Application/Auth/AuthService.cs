using EngagementApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngagementApi.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IAppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasherService _passwordHasher;

    public AuthService(IAppDbContext db, ITokenService tokenService, IPasswordHasherService passwordHasher)
    {
        _db = db;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _db.AdminUsers.SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
        if (user is null || !_passwordHasher.Verify(user, user.PasswordHash, password))
        {
            return null;
        }

        var (token, expiresAt) = _tokenService.CreateToken(user);
        return new LoginResponseDto { Token = token, ExpiresAt = expiresAt, Username = user.Username };
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _db.AdminUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null || !_passwordHasher.Verify(user, user.PasswordHash, currentPassword))
        {
            return false;
        }

        user.PasswordHash = _passwordHasher.Hash(user, newPassword);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
