using quebrantados.Exceptions.ValueObjects;

namespace quebrantados.ValueObjects;

public class Title : ValueObject
{

    public Title(string value)
    {
        var normalized = value.Trim();
        InvalidTitleException.ThrowIfInvalid(normalized);
        Value = normalized;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Title title) => title.Value;
    public static implicit operator Title(string value) => new(value);
}