using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;

namespace AdminPanel.BuildingConfiguration.Command.Persistence.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _repository;

    public EventStore(IEventStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _repository.FindByAggregateId(aggregateId);

        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
            throw new Exception();

        var version = expectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(BuildingAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await _repository.SaveAsync(eventModel);
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _repository.FindByAggregateId(aggregateId);

        if (eventStream == null || !eventStream.Any())
                throw new Exception("Incorrect ID provided!");

        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
    }
}