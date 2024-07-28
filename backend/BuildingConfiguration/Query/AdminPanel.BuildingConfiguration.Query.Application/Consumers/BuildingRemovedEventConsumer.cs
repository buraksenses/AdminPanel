using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using CQRS.Core.Messages;
using MassTransit;

namespace AdminPanel.BuildingConfiguration.Query.Application.Consumers;

public class BuildingRemovedEventConsumer : IConsumer<RemoveBuildingMessage>
{
    private readonly IBuildingRepository _repository;

    public BuildingRemovedEventConsumer(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<RemoveBuildingMessage> context)
    {
        var message = context.Message;

        var building = await _repository.GetByIdAsync(message.Id);

        if (building != null)
            await _repository.DeleteAsync(building);
    }
}