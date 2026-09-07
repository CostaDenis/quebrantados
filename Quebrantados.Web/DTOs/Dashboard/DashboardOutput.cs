using Quebrantados.Web.DTOs.Posts;

namespace Quebrantados.Web.DTOs.Dashboard;

public class DashboardOutput(
    int postCount,
    int publishedPostCount,
    int draftPostCount,
    int categoryCount,
    int emptyCategoryCount,
    int tagCount,
    int unusedTagCount,
    int recentUpdateCount,
    List<PostListItem> recentPosts,
    List<CategorySummaryItem> categorySummary)
{
    public int PostCount { get; init; } = postCount;
    public int PublishedPostCount { get; init; } = publishedPostCount;
    public int DraftPostCount { get; init; } = draftPostCount;
    public int CategoryCount { get; init; } = categoryCount;
    public int EmptyCategoryCount { get; init; } = emptyCategoryCount;
    public int TagCount { get; init; } = tagCount;
    public int UnusedTagCount { get; init; } = unusedTagCount;
    public int RecentUpdateCount { get; init; } = recentUpdateCount;
    public List<PostListItem> RecentPosts { get; init; } = recentPosts;
    public List<CategorySummaryItem> CategorySummary { get; init; } = categorySummary;
}
