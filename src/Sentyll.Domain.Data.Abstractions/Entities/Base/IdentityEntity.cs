namespace Sentyll.Domain.Data.Abstractions.Entities.Base;

public abstract class IdentityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
}