using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using AdminPanel.Shared.DTOs;
using CQRS.Core.Handlers;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Handlers;

public class RemoveBuildingCommandHandler : IRequestHandler<RemoveBuildingCommand, Response<RemoveBuildingDto>>
{
    private readonly IEventSourcingHandler<BuildingAggregate> _eventSourcingHandler;

    public RemoveBuildingCommandHandler(IEventSourcingHandler<BuildingAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }
    
    public async Task<Response<RemoveBuildingDto>> Handle(RemoveBuildingCommand request, CancellationToken cancellationToken)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(request.BuildingId);
        aggregate.RemoveBuilding(request.BuildingId);
        await _eventSourcingHandler.SaveAsync(aggregate);
        return Response<RemoveBuildingDto>.Success(new RemoveBuildingDto(request.BuildingId), 204);
    }
}