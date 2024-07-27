namespace AdminPanel.Shared.Entities;

public abstract class AuditableBaseEntity : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public Guid? LastModifiedBy { get; set; }
}