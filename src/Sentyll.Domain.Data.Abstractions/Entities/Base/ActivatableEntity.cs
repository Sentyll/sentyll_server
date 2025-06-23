using System.ComponentModel;

namespace Sentyll.Domain.Data.Abstractions.Entities.Base;

public abstract class ActivatableEntity : IdentityEntity
{
    [Required]
    [DefaultValue(true)]
    public bool IsEnabled { get; set; }
}