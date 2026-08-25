using quebrantados.Exceptions.ValueObjects;

namespace quebrantados.ValueObjects;

public class Summary : ValueObject
{

    public Summary(string value)
    {
        var normalized = value.Trim();
        InvalidSummaryException.ThrowIfInvalid(normalized);
        Value = normalized;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Summary summary) => summary.Value;
    public static implicit operator Summary(string value) => new(value);
}