using quebrantados.Exceptions.ValueObjects;

namespace quebrantados.ValueObjects;

public class Body : ValueObject
{

    public Body(string value)
    {
        var normalized = value.Trim();
        InvalidBodyException.ThrowIfInvalid(normalized);
        Value = normalized;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Body body) => body.Value;
    public static implicit operator Body(string value) => new(value);
}