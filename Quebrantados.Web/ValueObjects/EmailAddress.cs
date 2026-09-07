using Quebrantados.Web.Exceptions.ValueObjects;

namespace Quebrantados.Web.ValueObjects;

public class EmailAddress : ValueObject
{

    public EmailAddress(string value)
    {
        var normalized = value?.Trim().ToLowerInvariant()
            ?? string.Empty;
        InvalidEmailAddressException.ThrowIfInvalid(normalized);

        Value = normalized;
    }

    public string Value { get; }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;
    public static implicit operator EmailAddress(string value) => new(value);
}