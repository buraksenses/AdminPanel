using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Identity.Domain.Entities;

public class User : IdentityUser
{
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}