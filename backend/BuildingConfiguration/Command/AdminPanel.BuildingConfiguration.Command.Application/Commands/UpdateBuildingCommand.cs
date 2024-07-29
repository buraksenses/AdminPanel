using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Commands;

public record UpdateBuildingCommand(Guid Id, decimal BuildingCost, int ConstructionTime) : IRequest<Response<UpdateBuildingDto>>;