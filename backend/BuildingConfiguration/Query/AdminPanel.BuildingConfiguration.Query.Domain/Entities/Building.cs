using CQRS.Core.Domain.Enums;

namespace AdminPanel.BuildingConfiguration.Query.Domain.Entities;

public class Building
{
    public Guid Id { get; set; }

    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}