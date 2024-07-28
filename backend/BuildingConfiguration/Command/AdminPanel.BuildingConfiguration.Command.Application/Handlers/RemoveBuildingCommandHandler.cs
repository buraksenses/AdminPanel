using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using CQRS.Core.Handlers;
using CQRS.Core.Messages;
using MassTransit;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Handlers;

public class RemoveBuildingCommandHandler : IRequestHandler<RemoveBuildingCommand, Shared.DTOs.Response<RemoveBuildingDto>>
{
    private readonly IEventSourcingHandler<BuildingAggregate> _eventSourcingHandler;
    private readonly IPublishEndpoint _publishEndpoint;

    public RemoveBuildingCommandHandler(IEventSourcingHandler<BuildingAggregate> eventSourcingHandler, IPublishEndpoint publishEndpoint)
    {
        _eventSourcingHandler = eventSourcingHandler;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<Shared.DTOs.Response<RemoveBuildingDto>> Handle(RemoveBuildingCommand request, CancellationToken cancellationToken)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(request.BuildingId);
        aggregate.RemoveBuilding(request.BuildingId);
        await _eventSourcingHandler.SaveAsync(aggregate);
        
        await _publishEndpoint.Publish<RemoveBuildingMessage>(new RemoveBuildingMessage(aggregate.Id));
        
        return Shared.DTOs.Response<RemoveBuildingDto>.Success(new RemoveBuildingDto(request.BuildingId), 200);
    }
}