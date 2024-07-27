using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using AdminPanel.Shared.DTOs;
using CQRS.Core.Handlers;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Handlers;

public class AddBuildingCommandHandler : IRequestHandler<AddBuildingCommand, Response<AddBuildingDto>>
{
    private readonly IEventSourcingHandler<BuildingAggregate> _eventSourcingHandler;

    public AddBuildingCommandHandler(IEventSourcingHandler<BuildingAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }
    
    public async Task<Response<AddBuildingDto>> Handle(AddBuildingCommand request, CancellationToken cancellationToken)
    {
        var aggregate = new BuildingAggregate(Guid.NewGuid(), request.BuildingType, request.BuildingCost, request.ConstructionTime);
        
        await _eventSourcingHandler.SaveAsync(aggregate);
        
        return Response<AddBuildingDto>.Success(
            new AddBuildingDto(request.BuildingType,request.BuildingCost,request.ConstructionTime),
            201);
    }
}