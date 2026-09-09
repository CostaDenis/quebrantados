using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Entities;

public class Comment : Entity
{
    private Comment()
    { }

    public Comment(Guid postId, CommentAuthorName authorName,
        EmailAddress? authorEmail, CommentContent content)
    {
        PostWasNotInformedException.ThrowIfInvalid(postId);
        ArgumentNullException.ThrowIfNull(authorName);
        ArgumentNullException.ThrowIfNull(content);

        PostId = postId;
        AuthorName = authorName;
        AuthorEmail = authorEmail;
        Content = content;
        Status = ECommentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid PostId { get; private set; }
    public Post Post { get; private set; } = null!;
    public CommentAuthorName AuthorName { get; private set; } = null!;
    public EmailAddress? AuthorEmail { get; private set; }
    public CommentContent Content { get; private set; } = null!;

    public ECommentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void Approve() => Status = ECommentStatus.Approved;
    public void Reject() => Status = ECommentStatus.Rejected;
}