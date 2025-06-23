namespace Sentyll.Domain.Data.Abstractions.Entities.Base;

public abstract class JobEntity : IdentityEntity
{
    
    [StringLength(512)]
    public string? Function { get; set; }
    
    [StringLength(512)]
    public string? Description { get; set; }
    
    [StringLength(512)]
    public string? InitIdentifier { get;  set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public DateTime UpdatedAt { get; set; }
}