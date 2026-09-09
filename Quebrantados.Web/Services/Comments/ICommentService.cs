using Quebrantados.Web.DTOs.Comments;
using Quebrantados.Web.Enums;

namespace Quebrantados.Web.Services.Comments;

public interface ICommentService
{
    Task<List<CommentOutput>> GetApprovedByPostIdAsync(Guid postId, CancellationToken cancellationToken);
    Task<List<CommentAdminOutput>> GetByStatusAsync(ECommentStatus status, CancellationToken cancellationToken);
    Task<List<CommentAdminOutput>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(CommentInput input, CancellationToken cancellationToken);
    Task ApproveAsync(Guid id, CancellationToken cancellationToken);
    Task RejectAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}