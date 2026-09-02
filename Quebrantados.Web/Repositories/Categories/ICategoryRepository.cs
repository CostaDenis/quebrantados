using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CategoryListItem>> GetAllWithPostCountAsync(CancellationToken cancellationToken);
    Task CreateAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(Category category, CancellationToken cancellationToken);
    Task<bool> ExistsByNameOrSlugAsync(string name, string slug,
        CancellationToken cancellationToken, Guid? excludedId = null);
    Task<bool> HasPostsAsync(Guid categoryId, CancellationToken cancellationToken);
}