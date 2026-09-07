using Quebrantados.Web.Exceptions.ValueObjects;

namespace Quebrantados.Web.ValueObjects;

public class CommentAuthorName : ValueObject
{

    public CommentAuthorName(string value)
    {
        var normalized = value?.Trim() ?? string.Empty;
        InvalidCommentAuthorNameException.ThrowIfInvalid(normalized);

        Value = normalized;
    }

    public string Value { get; }
    public override string ToString() => Value;
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(CommentAuthorName commentAuthorName) => commentAuthorName.Value;
    public static implicit operator CommentAuthorName(string value) => new(value);
}