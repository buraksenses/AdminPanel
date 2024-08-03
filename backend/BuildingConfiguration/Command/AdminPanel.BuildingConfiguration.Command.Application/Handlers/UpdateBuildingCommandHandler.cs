using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Messages;
using MassTransit;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Handlers;

public class UpdateBuildingCommandHandler : IRequestHandler<UpdateBuildingCommand, Shared.DTOs.Response<UpdateBuildingDto>>
{
    private readonly IEventSourcingHandler<BuildingAggregate> _eventSourcingHandler;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateBuildingCommandHandler(IEventSourcingHandler<BuildingAggregate> eventSourcingHandler, IPublishEndpoint publishEndpoint)
    {
        _eventSourcingHandler = eventSourcingHandler;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<Shared.DTOs.Response<UpdateBuildingDto>> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(request.Id);
        aggregate.UpdateBuilding(new BuildingUpdatedEvent
        {
            BuildingCost = request.BuildingCost,
            ConstructionTime = request.ConstructionTime,
            Id = request.Id
        });
        await _eventSourcingHandler.SaveAsync(aggregate);

        await _publishEndpoint.Publish<UpdateBuildingMessage>(new UpdateBuildingMessage(aggregate.Id,
            request.BuildingCost, request.ConstructionTime));
        
        return Shared.DTOs.Response<UpdateBuildingDto>.Success(new UpdateBuildingDto(request.BuildingCost, request.ConstructionTime),200);
    }
}