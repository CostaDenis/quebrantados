using Quebrantados.Web.DTOs.Categories;

namespace Quebrantados.Web.Services.Categories;

public interface ICategoryService
{
    Task<CategoryOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CategoryListItem>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(CreateCategoryInput input, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, EditCategoryInput input, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}