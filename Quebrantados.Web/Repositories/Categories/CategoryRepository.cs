using Microsoft.EntityFrameworkCore;
using Quebrantados.Web.Data;
using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Repositories.Categories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<CategoryListItem>> GetAllWithPostCountAsync(CancellationToken cancellationToken)
        => await context.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryListItem(
                category.Id,
                category.Name,
                category.Slug,
                category.Posts.Count))
            .ToListAsync(cancellationToken);

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

    public async Task<bool> ExistsByNameOrSlugAsync(string name, string slug, CancellationToken cancellationToken, Guid? excludedId = null)
    {
        CategoryName normalizedName = name;
        Slug normalizedSlug = slug;

        return await context.Categories.AnyAsync(
            category => (!excludedId.HasValue || category.Id != excludedId.Value)
                && (category.Name == normalizedName || category.Slug == normalizedSlug), cancellationToken);
    }

    public async Task<bool> HasPostsAsync(Guid categoryId, CancellationToken cancellationToken)
        => await context.Posts
            .AnyAsync(x => x.Category.Id == categoryId, cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken)
        => await context.Categories.CountAsync(cancellationToken);
}