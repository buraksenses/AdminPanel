using AdminPanel.Identity.Domain;
using AdminPanel.Identity.Domain.Entities;
using AdminPanel.Identity.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Identity.Persistence.Context;

public class UserDbContext : IdentityDbContext<User>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Seed();
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
}