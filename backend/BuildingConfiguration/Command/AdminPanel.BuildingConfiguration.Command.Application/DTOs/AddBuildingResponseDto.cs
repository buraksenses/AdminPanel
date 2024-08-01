using CQRS.Core.Domain.Enums;

namespace AdminPanel.BuildingConfiguration.Command.Application.DTOs;

public record AddBuildingResponseDto(Guid Id, BuildingType BuildingType, decimal BuildingCost, int ConstructionTime);