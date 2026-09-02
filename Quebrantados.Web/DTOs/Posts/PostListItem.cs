namespace Quebrantados.Web.DTOs.Posts;

public class PostListItem(string title, string categoryName,
    string status, DateTime lastUpdateDate)
{
    public string Title { get; init; } = title;
    public string CategoryName { get; init; } = categoryName;
    public string Status { get; init; } = status;
    public DateTime LastUpdateDate { get; init; } = lastUpdateDate;
}