﻿namespace AdminPanel.BuildingConfiguration.Query.Application.DTOs;

public record BuildingDto(Guid Id, string BuildingType, decimal BuildingCost, int ConstructionTime);