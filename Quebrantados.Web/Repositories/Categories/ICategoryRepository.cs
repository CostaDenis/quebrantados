using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Categories;

public interface ICategoryRepository
{
    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
    public Task CreateAsync(Category category, CancellationToken cancellationToken);
    public Task UpdateAsync(Category category, CancellationToken cancellationToken);
    public Task DeleteAsync(Category category, CancellationToken cancellationToken);
}