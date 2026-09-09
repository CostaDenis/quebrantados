using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Enums;

namespace Quebrantados.Web.Repositories.Posts;

public interface IPostRepository
{
    public Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<Post?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    public Task<List<Post>> GetAllAsync(CancellationToken cancellationToken);
    public Task CreateAsync(Post post, CancellationToken cancellationToken);
    public Task UpdateAsync(Post post, CancellationToken cancellationToken);
    public Task DeleteAsync(Post post, CancellationToken cancellationToken);
    Task<bool> ExistsTitleOrSlugAsync(string title, string slug, CancellationToken cancellationToken, Guid? excludedId = null);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<int> CountByStatusAsync(EPostStatus status, CancellationToken cancellationToken);
    Task<int> CountUpdatedSinceAsync(DateTime since, CancellationToken cancellationToken);
    Task<List<PostListItem>> GetRecentAsync(DateTime since, int limit, CancellationToken cancellationToken);
    Task<bool> ExistsPublishedAsync(Guid id, CancellationToken cancellationToken);
}
