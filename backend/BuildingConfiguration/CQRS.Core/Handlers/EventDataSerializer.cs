using CQRS.Core.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AdminPanel.BuildingConfiguration.Command.Persistence.Config;

public class EventDataSerializer : SerializerBase<BaseEvent>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BaseEvent value)
    {
        var type = value.GetType();
        var serializer = BsonSerializer.LookupSerializer(type);
        serializer.Serialize(context, args, value);
    }

    public override BaseEvent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var typeDiscriminator = context.Reader.ReadString();
        var type = Type.GetType(typeDiscriminator);
        var serializer = BsonSerializer.LookupSerializer(type);
        return (BaseEvent)serializer.Deserialize(context, args);
    }
}