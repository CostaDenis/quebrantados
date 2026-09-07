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

        var categoryCount = await categoryRepository.CountAsync(cancellationToken);
        var tagCount = await tagRepository.CountAsync(cancellationToken);

        var recentPosts = await postRepository
            .GetRecentAsync(since, 4, cancellationToken);

        return new DashboardOutput(postCount, publishedPostCount, draftPostCount,
            categoryCount, tagCount, recentPosts);
    }
}