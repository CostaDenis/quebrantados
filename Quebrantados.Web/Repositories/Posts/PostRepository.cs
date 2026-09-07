using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Enums;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Repositories.Posts;

public class PostRepository(AppDbContext context) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Posts
            .Include(x => x.Category)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Post>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Posts
            .Include(post => post.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<bool> ExistsTitleOrSlugAsync(string title, string slug, CancellationToken cancellationToken, Guid? excludedId = null)
    {
        Title normalizedTitle = title;
        Slug normalizedSlug = slug;

        return await context.Posts.AnyAsync(post =>
            (!excludedId.HasValue || post.Id != excludedId.Value)
                && (post.Title == normalizedTitle || post.Slug == normalizedSlug),
            cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
        => await context.Posts.CountAsync(cancellationToken);

    public async Task<int> CountByStatusAsync(EPostStatus status, CancellationToken cancellationToken)
        => await context.Posts.Where(x => x.Status == status).CountAsync(cancellationToken);

    public async Task<List<PostListItem>> GetRecentAsync(DateTime since, int limit, CancellationToken cancellationToken)
        => await context.Posts
        .AsNoTracking()
        .Where(x => x.LastUpdateDate >= since)
        .OrderByDescending(post => post.LastUpdateDate)
        .Take(limit)
        .Select(post => new PostListItem(post.Id, post.Title.Value, post.Slug.Value,
            post.Category.Name.Value, post.Status, post.LastUpdateDate))
        .ToListAsync(cancellationToken);
}