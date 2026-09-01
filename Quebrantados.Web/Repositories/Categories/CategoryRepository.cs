using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Categories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Categories.AsNoTracking().ToListAsync(cancellationToken);

    public async Task CreateAsync(Category category, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
    }

}