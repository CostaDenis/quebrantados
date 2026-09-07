using Quebrantados.Web.Enums;

namespace Quebrantados.Web.DTOs.Posts;

public class PostOutput(
    Guid id,
    string title,
    string slug,
    string? summary,
    string body,
    DateTime createdAt,
    DateTime lastUpdateDate,
    Guid categoryId,
    string categoryName,
    EPostStatus status,
    DateTime? publishedAt,
    List<Guid> tagIds)
{
    public Guid Id { get; init; } = id;
    public string Title { get; init; } = title;
    public string Slug { get; init; } = slug;
    public string? Summary { get; init; } = summary;
    public string Body { get; init; } = body;
    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime LastUpdateDate { get; init; } = lastUpdateDate;
    public Guid CategoryId { get; init; } = categoryId;
    public string CategoryName { get; init; } = categoryName;
    public EPostStatus Status { get; init; } = status;
    public DateTime? PublishedAt { get; init; } = publishedAt;
    public List<Guid> TagIds { get; init; } = tagIds;
}
