using CQRS.Core.Events;

namespace EcoVerse.StockManagement.Common.Events;

public class InventoryItemRemovedEvent : BaseEvent
{
    public Guid ItemId { get; set; }
    
    public InventoryItemRemovedEvent() : base(nameof(InventoryItemRemovedEvent))
    {
    }
}