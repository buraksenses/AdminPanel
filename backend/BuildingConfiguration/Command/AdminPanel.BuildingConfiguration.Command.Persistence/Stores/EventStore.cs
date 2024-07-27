using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;

namespace AdminPanel.BuildingConfiguration.Command.Persistence.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _repository;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository repository, IEventProducer eventProducer)
    {
        _repository = repository;
        _eventProducer = eventProducer;
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

            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

            await _eventProducer.ProduceAsync(topic, @event);
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