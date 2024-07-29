namespace CQRS.Core.Messages;

public record UpdateBuildingMessage(Guid Id, decimal BuildingCost, int ConstructionTime) : Message(Id);