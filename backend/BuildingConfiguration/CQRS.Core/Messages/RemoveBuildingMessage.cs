namespace CQRS.Core.Messages;

public record RemoveBuildingMessage(Guid Id) : Message(Id);