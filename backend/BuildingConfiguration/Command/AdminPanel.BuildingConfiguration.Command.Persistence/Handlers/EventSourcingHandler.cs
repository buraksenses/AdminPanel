using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;

namespace AdminPanel.BuildingConfiguration.Command.Persistence.Handlers;

public class EventSourcingHandler : IEventSourcingHandler<BuildingAggregate>
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }
    
    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }

    public async Task<BuildingAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new BuildingAggregate();

        var events = await _eventStore.GetEventsAsync(aggregateId);

        if (events == null || !events.Any())
            return aggregate;
        
        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(x => x.Version).Max();

        return aggregate;
    }
}