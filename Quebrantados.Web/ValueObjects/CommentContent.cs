using Quebrantados.Web.Exceptions.ValueObjects;

namespace Quebrantados.Web.ValueObjects;

public class CommentContent : ValueObject
{

    public CommentContent(string value)
    {
        var normalized = value?.Trim() ?? string.Empty;
        InvalidCommentContentException.ThrowIfInvalid(normalized);
        Value = normalized;
    }

    public string Value { get; }

    public override string ToString() => Value;
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(CommentContent commentContent) => commentContent.Value;
    public static implicit operator CommentContent(string value) => new(value);
}