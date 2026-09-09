using System;
namespace Quebrantados.Web.DTOs.Comments;

public class CommentOutput(Guid id, string authorName,
    string content, DateTime createdAt)
{
    public Guid Id { get; init; } = id;
    public string AuthorName { get; init; } = authorName;
    public string Content { get; init; } = content;
    public DateTime CreatedAt { get; init; } = createdAt;
}