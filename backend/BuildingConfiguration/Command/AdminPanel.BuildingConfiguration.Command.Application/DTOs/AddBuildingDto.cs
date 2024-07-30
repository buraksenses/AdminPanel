using CQRS.Core.Domain.Enums;

namespace AdminPanel.BuildingConfiguration.Command.Application.DTOs;

public record AddBuildingDto(BuildingType BuildingType, decimal BuildingCost, int ConstructionTime);