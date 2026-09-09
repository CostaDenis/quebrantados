using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Enums;

namespace Quebrantados.Web.Repositories.Comments;

public class CommentRepository(AppDbContext context)
    : ICommentRepository
{

    public async Task<List<Comment>> GetApprovedByPostIdAsync(Guid postId, CancellationToken cancellationToken)
        => await context.Comments
            .AsNoTracking()
            .Where(x => x.PostId == postId && x.Status == ECommentStatus.Approved)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<List<Comment>> GetByStatusAsync(ECommentStatus status, CancellationToken cancellationToken)
        => await context.Comments
                .AsNoTracking()
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Comments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Comments.ToListAsync(cancellationToken);

    public async Task CreateAsync(Comment comment, CancellationToken cancellationToken)
    {
        await context.AddAsync(comment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken)
    {
        context.Update(comment);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Comment comment, CancellationToken cancellationToken)
    {
        context.Remove(comment);
        await context.SaveChangesAsync(cancellationToken);
    }

}