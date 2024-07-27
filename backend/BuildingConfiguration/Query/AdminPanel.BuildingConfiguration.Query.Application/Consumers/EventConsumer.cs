using System.Text.Json;
using AdminPanel.BuildingConfiguration.Query.Persistence.Converters;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdminPanel.BuildingConfiguration.Query.Application.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly IMediator _mediator;
    private readonly ILogger<EventConsumer> _logger;
    private readonly ConsumerConfig _config;

    public EventConsumer(IOptions<ConsumerConfig> config, IMediator mediator, ILogger<EventConsumer> logger)
    {
        _config = config.Value;
        _mediator = mediator;
        _logger = logger;
    }
    
    public void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();
        consumer.Subscribe(topic);
        
        while (true)
        {
            try
            { 
                var consumeResult = consumer.Consume(); 
                if (consumeResult?.Message == null) 
                    continue;
                
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

                var eventName = @event.Type;

                var eventType = ResolveEventType(eventName);
                
                HandleMessage(consumeResult.Message.Value, eventType);
                
                _logger.LogInformation(eventName);
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Consume error: {e.Error.Reason}");
            }
        }
       
    }
    
    private async void HandleMessage(string message, Type eventType)
    {
        try
        {
            var notification = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) as INotification;
            if (notification != null)
            {
                await _mediator.Publish(notification);
            }
        }
        catch (JsonException e)
        {
            Console.WriteLine($"JSON Error: {e.Message}");
        }
    }
    
    private static Type ResolveEventType(string eventTypeName)
    {
        var fullyQualifiedTypeName = $"AdminPanel.BuildingConfiguration.Query.Application.Notifications.{eventTypeName}, AdminPanel.BuildingConfiguration.Query.Application";
        
        var eventType = Type.GetType(fullyQualifiedTypeName);

        if (eventType == null)
        {
            Console.WriteLine("Event type not found.");
        }

        return eventType;
    }
}