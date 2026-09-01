using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Posts;

public class PostRepository(AppDbContext context) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Posts
            .Include(x => x.Category)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Post>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Posts.AsNoTracking().ToListAsync(cancellationToken);

    public async Task CreateAsync(Post post, CancellationToken cancellationToken)
    {
        await context.Posts.AddAsync(post, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Post post, CancellationToken cancellationToken)
    {
        context.Posts.Update(post);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Post post, CancellationToken cancellationToken)
    {
        context.Posts.Remove(post);
        await context.SaveChangesAsync(cancellationToken);
    }

}