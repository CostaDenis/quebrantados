using Quebrantados.Web.Exceptions.ValueObjects;

namespace Quebrantados.Web.ValueObjects;

public class TagName : ValueObject
{

    public TagName(string value)
    {
        var normalized = value.Trim();
        InvalidTagNameException.ThrowIfInvalid(normalized);
        Value = normalized;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(TagName tagName) => tagName.Value;
    public static implicit operator TagName(string value) => new(value);
}