using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Commands;

public record RemoveBuildingCommand(Guid BuildingId) : IRequest<Response<RemoveBuildingDto>>;