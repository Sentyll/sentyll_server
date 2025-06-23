using System.ComponentModel.DataAnnotations;

namespace Sentyll.Domain.Common.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OrderByAttribute : ValidationAttribute
{
    
    private readonly string[] _orderables;

    public OrderByAttribute(params string[] orderables)
    {
        if (orderables == null || orderables.Length == 0)
            throw new ArgumentException("At least one column must be provided for ordering.", nameof(orderables));
        
        _orderables = orderables;
    }

    public override bool IsValid(object? value)
    {
        return value is string stringValue && 
               _orderables.Any(orderBy => string.Equals(orderBy, stringValue, StringComparison.InvariantCultureIgnoreCase));
    }

    public override string FormatErrorMessage(string name)
    {
        var allowedOrderables = string.Join(", ", _orderables);
        return $"{name} is not a valid order by field, please use one of the following: {allowedOrderables}";
    }
}