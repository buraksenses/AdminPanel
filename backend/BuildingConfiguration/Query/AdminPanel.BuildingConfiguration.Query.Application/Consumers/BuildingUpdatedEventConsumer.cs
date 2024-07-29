using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using CQRS.Core.Messages;
using MassTransit;

namespace AdminPanel.BuildingConfiguration.Query.Application.Consumers;

public class BuildingUpdatedEventConsumer : IConsumer<UpdateBuildingMessage>
{
    private readonly IBuildingRepository _repository;

    public BuildingUpdatedEventConsumer(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<UpdateBuildingMessage> context)
    {
        var message = context.Message;

        var building = await _repository.GetByIdAsync(message.Id);

        if (building == null)
            throw new Exception("building not found!");

        building.BuildingCost = message.BuildingCost;
        building.ConstructionTime = message.ConstructionTime;

        await _repository.UpdateAsync(building);
    }
}