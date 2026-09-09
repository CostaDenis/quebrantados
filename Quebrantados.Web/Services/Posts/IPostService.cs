using Quebrantados.Web.DTOs.Posts;

namespace Quebrantados.Web.Services.Posts;

public interface IPostService
{
    Task<PostOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PostOutput?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<List<PostListItem>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(CreatePostInput input, bool publish, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, EditPostInput input, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task PublishAsync(Guid id, CancellationToken cancellationToken);
    Task MoveToDraftAsync(Guid id, CancellationToken cancellationToken);
}