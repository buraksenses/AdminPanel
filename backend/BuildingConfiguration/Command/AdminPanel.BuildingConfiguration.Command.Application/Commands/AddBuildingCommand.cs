using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.Shared.DTOs;
using CQRS.Core.Domain.Enums;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Commands;

public record AddBuildingCommand(BuildingType BuildingType, decimal BuildingCost, int ConstructionTime) : IRequest<Response<AddBuildingResponseDto>>;