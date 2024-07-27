using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Notifications;

public class BuildingRemovedNotification : INotification
{
    public Guid Id { get; set; }
}