using AdminPanel.BuildingConfiguration.Command.Domain.Enums;
using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.BuildingConfiguration.Query.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyEnumConversions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.Property(e => e.BuildingType)
                .HasConversion(
                    v => v.ToString(),  
                    v => (BuildingType)Enum.Parse(typeof(BuildingType), v));

            entity.Property(e => e.BuildingCost).HasPrecision(24, 6);
        });
    }
}