using Quebrantados.Web.Exceptions.ValueObjects;

namespace Quebrantados.Web.ValueObjects;

public class CategoryName : ValueObject
{

    public CategoryName(string value)
    {
        var normalized = value.Trim();
        InvalidCategoryNameException.ThrowIfInvalid(normalized);
        Value = normalized;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(CategoryName categoryName) => categoryName.Value;
    public static implicit operator CategoryName(string value) => new(value);
}
