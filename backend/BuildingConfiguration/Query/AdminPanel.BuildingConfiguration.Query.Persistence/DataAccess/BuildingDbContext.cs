using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;

public class BuildingDbContext : DbContext
{
    public BuildingDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyEnumConversions();
    }

    public DbSet<Building> Buildings { get; set; }
}