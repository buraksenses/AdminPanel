using AdminPanel.BuildingConfiguration.Command.Domain.Enums;

namespace CQRS.Core.Messages;

public class CreateBuildingMessage : Message
{
    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}