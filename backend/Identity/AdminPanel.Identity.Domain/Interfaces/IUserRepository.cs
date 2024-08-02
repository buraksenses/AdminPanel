using AdminPanel.Identity.Domain.Entities;

namespace AdminPanel.Identity.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByRefreshToken(string? token);
    void RevokeRefreshToken(RefreshToken refreshToken);
    Task AddRefreshToken(User user, RefreshToken refreshToken);
    Task UpdateUserAsync(User user);
}