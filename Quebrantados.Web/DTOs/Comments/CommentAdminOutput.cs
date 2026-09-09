using Quebrantados.Web.Enums;

namespace Quebrantados.Web.DTOs.Comments;

public class CommentAdminOutput(Guid id, Guid postId, string authorName,
    string? authorEmail, string content,
    ECommentStatus status, DateTime createdAt)
{
    public Guid Id { get; init; } = id;
    public Guid PostId { get; init; } = postId;
    public string AuthorName { get; init; } = authorName;
    public string? AuthorEmail { get; init; } = authorEmail;
    public string Content { get; init; } = content;
    public ECommentStatus Status { get; init; } = status;
    public DateTime CreatedAt { get; init; } = createdAt;
}