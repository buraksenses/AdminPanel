using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using CQRS.Core.Messages;
using MassTransit;

namespace AdminPanel.BuildingConfiguration.Query.Application.Consumers;

public class BuildingCreatedEventConsumer : IConsumer<CreateBuildingMessage>
{
    private readonly IBuildingRepository _repository;

    public BuildingCreatedEventConsumer(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<CreateBuildingMessage> context)
    {
        var message = context.Message;

        await _repository.CreateAsync(new Building
        {
            Id = message.Id,
            BuildingCost = message.BuildingCost,
            BuildingType = message.BuildingType,
            ConstructionTime = message.ConstructionTime
        });
    }
}