namespace Quebrantados.Web.DTOs.Posts;

public class PostListItem(string title, string categoryName, DateTime lastUpdateDate)
{
    public string Title { get; init; } = title;
    public string CategoryName { get; init; } = categoryName;
    public DateTime LastUpdateDate { get; init; } = lastUpdateDate;
}