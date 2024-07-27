using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Identity.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        const string adminRoleId = "50df0d64-b2ae-482a-98d1-bd187fbbbeda";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}