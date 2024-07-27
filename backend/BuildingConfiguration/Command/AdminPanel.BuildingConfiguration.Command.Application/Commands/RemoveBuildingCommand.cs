using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Commands;

public class RemoveBuildingCommand : IRequest<Response<RemoveBuildingDto>>
{
    public Guid BuildingId { get; set; }
}