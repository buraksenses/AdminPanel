using CQRS.Core.Domain.Enums;

namespace AdminPanel.BuildingConfiguration.Command.Application.DTOs;

public record AddBuildingRequestDto(BuildingType BuildingType, decimal BuildingCost, int ConstructionTime);