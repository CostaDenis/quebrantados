using Quebrantados.Web.Entities;
using Quebrantados.Web.Enums;

namespace Quebrantados.Web.Repositories.Comments;

public interface ICommentRepository
{
    Task<List<Comment>> GetApprovedByPostIdAsync(Guid postId, CancellationToken cancellationToken);
    Task<List<Comment>> GetByStatusAsync(ECommentStatus status, CancellationToken cancellationToken);
    Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken);
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Comment comment, CancellationToken cancellationToken);
    Task UpdateAsync(Comment comment, CancellationToken cancellationToken);
    Task DeleteAsync(Comment comment, CancellationToken cancellationToken);
}