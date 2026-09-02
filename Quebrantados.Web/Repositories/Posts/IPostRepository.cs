using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Posts;

public interface IPostRepository
{
    public Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<Post>> GetAllAsync(CancellationToken cancellationToken);
    public Task CreateAsync(Post post, CancellationToken cancellationToken);
    public Task UpdateAsync(Post post, CancellationToken cancellationToken);
    public Task DeleteAsync(Post post, CancellationToken cancellationToken);
    Task<bool> ExistsTitleOrSlugAsync(string title, string slug, CancellationToken cancellationToken, Guid? excludedId = null);
}