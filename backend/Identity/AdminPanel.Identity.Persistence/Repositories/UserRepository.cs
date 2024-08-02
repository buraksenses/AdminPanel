using AdminPanel.Identity.Domain;
using AdminPanel.Identity.Domain.Entities;
using AdminPanel.Identity.Domain.Interfaces;
using AdminPanel.Identity.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Identity.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly UserDbContext _dbContext;

    public UserRepository(UserManager<User> userManager, UserDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetUserByRefreshToken(string? token)
    {
        return await _userManager.Users.Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
    }

    public void RevokeRefreshToken(RefreshToken refreshToken)
    {
        refreshToken.Revoked = DateTime.Now;
        _dbContext.RefreshTokens.Update(refreshToken);
    }

    public async Task AddRefreshToken(User user, RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        user.RefreshTokens.Add(refreshToken);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userManager.UpdateAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}