using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Tags;

public class TagRepository(AppDbContext context) : ITagRepository
{

    public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Tags.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Tags.AsNoTracking().ToListAsync(cancellationToken);

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
}