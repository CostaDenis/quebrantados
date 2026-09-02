using Quebrantados.Web.DTOs.Categories;

namespace Quebrantados.Web.Services.Categories;

public interface ICategoryService
{
    public Task<CategoryOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<CategoryListItem>> GetAllAsync(CancellationToken cancellationToken);
    public Task CreateAsync(CreateCategoryInput input, CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, EditCategoryInput input, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}