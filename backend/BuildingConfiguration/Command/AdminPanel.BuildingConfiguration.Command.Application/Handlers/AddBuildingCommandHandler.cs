using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using CQRS.Core.Handlers;
using CQRS.Core.Messages;
using MassTransit;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Handlers;

public class AddBuildingCommandHandler : IRequestHandler<AddBuildingCommand, Shared.DTOs.Response<AddBuildingResponseDto>>
{
    private readonly IEventSourcingHandler<BuildingAggregate> _eventSourcingHandler;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddBuildingCommandHandler(IEventSourcingHandler<BuildingAggregate> eventSourcingHandler, IPublishEndpoint publishEndpoint)
    {
        _eventSourcingHandler = eventSourcingHandler;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<Shared.DTOs.Response<AddBuildingResponseDto>> Handle(AddBuildingCommand request, CancellationToken cancellationToken)
    {
        var aggregate = new BuildingAggregate(Guid.NewGuid(), request.BuildingType, request.BuildingCost, request.ConstructionTime);
        
        await _eventSourcingHandler.SaveAsync(aggregate);

        await _publishEndpoint.Publish<CreateBuildingMessage>(new CreateBuildingMessage(aggregate.Id,
            request.BuildingType, request.BuildingCost, request.ConstructionTime));

        return Shared.DTOs.Response<AddBuildingResponseDto>.Success(
            new AddBuildingResponseDto(aggregate.Id, request.BuildingType, request.BuildingCost, request.ConstructionTime),
            201);
    }
}