namespace Quebrantados.Web.DTOs.Posts;

public class PostOutput(string title, string slug, string? summary, string body,
    DateTime createdAt, DateTime lastUpdateDate, string categoryName)
{
    public string Title { get; init; } = title;
    public string Slug { get; init; } = slug;
    public string? Summary { get; init; } = summary;
    public string Body { get; init; } = body;
    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime LastUpdateDate { get; init; } = lastUpdateDate;
    public string CategoryName { get; init; } = categoryName;
}