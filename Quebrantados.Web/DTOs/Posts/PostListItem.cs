using Quebrantados.Web.Enums;

namespace Quebrantados.Web.DTOs.Posts;

public class PostListItem(Guid id, string title, string slug,
    string categoryName, EPostStatus status, DateTime lastUpdateDate)
{
    public Guid Id { get; init; } = id;
    public string Title { get; init; } = title;
    public string Slug { get; init; } = slug;
    public string CategoryName { get; init; } = categoryName;
    public EPostStatus Status { get; init; } = status;
    public DateTime LastUpdateDate { get; init; } = lastUpdateDate;
}