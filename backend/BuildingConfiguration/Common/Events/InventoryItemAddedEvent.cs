using CQRS.Core.Events;

namespace Common.Events;

public class InventoryItemAddedEvent : BaseEvent
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public InventoryItemAddedEvent() : base(nameof(InventoryItemAddedEvent))
    {
    }
    
}