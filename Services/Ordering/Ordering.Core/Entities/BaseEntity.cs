namespace Ordering.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTime? EditedOn { get; set; }
}