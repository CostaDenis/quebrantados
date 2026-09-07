using Quebrantados.Web.DTOs.Posts;

namespace Quebrantados.Web.DTOs.Dashboard;

public class DashboardOutput(int postCount, int publishedPostCount, int draftPostCount, int categoryCount, int tagCount, List<PostListItem> recentPosts)
{
    public int PostCount { get; init; } = postCount;
    public int PublishedPostCount { get; init; } = publishedPostCount;
    public int DraftPostCount { get; init; } = draftPostCount;
    public int CategoryCount { get; init; } = categoryCount;
    public int TagCount { get; init; } = tagCount;
    public List<PostListItem> RecentPosts { get; init; } = recentPosts;
}