using AdminPanel.BuildingConfiguration.Command.Domain.Enums;

namespace AdminPanel.BuildingConfiguration.Query.Application.DTOs;

public record BuildingDto(BuildingType BuildingType, decimal BuildingCost, int ConstructionTime);