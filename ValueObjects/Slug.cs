using quebrantados.Exceptions.ValueObjects;

namespace quebrantados.ValueObjects;

public class Slug : ValueObject
{

    public Slug(string value)
    {
        var normalized = value.Trim().ToLowerInvariant();
        InvalidSlugException.ThrowIfInvalid(value, normalized);
        Value = normalized;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Slug slug) => slug.Value;
    public static implicit operator Slug(string value) => new(value);
}