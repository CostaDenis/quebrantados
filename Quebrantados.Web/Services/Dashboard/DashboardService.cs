using Quebrantados.Web.DTOs.Dashboard;
using Quebrantados.Web.Enums;
using Quebrantados.Web.Repositories.Categories;
using Quebrantados.Web.Repositories.Posts;
using Quebrantados.Web.Repositories.Tags;

namespace Quebrantados.Web.Services.Dashboard;

public class DashboardService(IPostRepository postRepository,
    ICategoryRepository categoryRepository,
    ITagRepository tagRepository) : IDashboardService
{
    public async Task<DashboardOutput> GetDataAsync(CancellationToken cancellationToken)
    {
        var since = DateTime.UtcNow.AddDays(-7);
        var postCount = await postRepository.CountAsync(cancellationToken);

        var publishedPostCount = await postRepository
            .CountByStatusAsync(EPostStatus.Published, cancellationToken);

        var draftPostCount = await postRepository
            .CountByStatusAsync(EPostStatus.Draft, cancellationToken);

        var categories = await categoryRepository
            .GetAllWithPostCountAsync(cancellationToken);

        var categoryCount = categories.Count;
        var emptyCategoryCount = categories.Count(category => category.PostCount == 0);

        var mostUsedCategories = categories
            .OrderByDescending(category => category.PostCount)
            .ThenBy(category => category.Name)
            .Take(4)
            .ToList();

        var highestPostCount = mostUsedCategories.FirstOrDefault()?.PostCount ?? 0;

        var categorySummary = mostUsedCategories
            .Select(category => new CategorySummaryItem(
                category.Name,
                category.PostCount,
                highestPostCount == 0
                    ? 0
                    : (int)Math.Round(category.PostCount * 100d / highestPostCount)))
            .ToList();

        var tagCount = await tagRepository.CountAsync(cancellationToken);
        var unusedTagCount = await tagRepository.CountUnusedAsync(cancellationToken);

        var recentUpdateCount = await postRepository
            .CountUpdatedSinceAsync(since, cancellationToken);

        var recentPosts = await postRepository
            .GetRecentAsync(since, 4, cancellationToken);

        return new DashboardOutput(postCount, publishedPostCount, draftPostCount,
            categoryCount, emptyCategoryCount, tagCount, unusedTagCount,
            recentUpdateCount, recentPosts, categorySummary);
    }
}
