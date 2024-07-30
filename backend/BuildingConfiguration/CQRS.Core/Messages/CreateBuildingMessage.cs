using CQRS.Core.Domain.Enums;

namespace CQRS.Core.Messages;

public record CreateBuildingMessage(Guid Id, BuildingType BuildingType, decimal BuildingCost, int ConstructionTime) : Message(Id);