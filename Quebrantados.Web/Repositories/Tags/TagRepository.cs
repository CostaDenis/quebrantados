using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Repositories.Tags;

public class TagRepository(AppDbContext context) : ITagRepository
{

    public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Tags.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<TagListItem>> GetAllWithPostCountAsync(CancellationToken cancellationToken)
        => await context.Tags
            .AsNoTracking()
            .OrderBy(tag => tag.Name)
            .Select(tag => new TagListItem(
                tag.Id,
                tag.Name,
                tag.Slug,
                tag.Posts.Count))
            .ToListAsync(cancellationToken);

    public async Task CreateAsync(Tag tag, CancellationToken cancellationToken)
    {
        await context.Tags.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tag tag, CancellationToken cancellationToken)
    {
        context.Tags.Update(tag);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Tag tag, CancellationToken cancellationToken)
    {
        context.Tags.Remove(tag);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameOrSlugAsync(
        string name,
        string slug,
        CancellationToken cancellationToken,
        Guid? excludedId = null)
    {
        TagName normalizedName = name;
        Slug normalizedSlug = slug;

        return await context.Tags.AnyAsync(
            tag => (!excludedId.HasValue || tag.Id != excludedId.Value)
                && (tag.Name == normalizedName || tag.Slug == normalizedSlug),
            cancellationToken);
    }
}
