using CQRS.Core.Domain.Enums;
using CQRS.Core.Events;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CQRS.Core.Handlers;

public class EventDataSerializer : SerializerBase<BaseEvent>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BaseEvent value)
        {
            var bsonWriter = context.Writer;

            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString("Type", value.Type);

            var serializer = BsonSerializer.LookupSerializer(value.GetType());
            bsonWriter.WriteName("EventData");
            serializer.Serialize(context, args, value);

            bsonWriter.WriteEndDocument();
        }

        public override BaseEvent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;

            bsonReader.ReadStartDocument();

            var typeName = bsonReader.ReadString("Type");
            var fullTypeName = $"CQRS.Core.Events.{typeName}, CQRS.Core";
            var eventType = Type.GetType(fullTypeName);

            if (eventType == null)
            {
                bsonReader.ReadEndDocument();
                throw new Exception($"Unknown event type: {typeName}");
            }

            bsonReader.ReadName("EventData");
            var serializer = BsonSerializer.LookupSerializer(eventType);
            var @event = (BaseEvent)serializer.Deserialize(context, args);

            bsonReader.ReadEndDocument();

            return @event;
        }

}