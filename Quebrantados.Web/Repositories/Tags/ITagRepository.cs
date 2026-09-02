using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Tags;

public interface ITagRepository
{
    public Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<TagListItem>> GetAllWithPostCountAsync(CancellationToken cancellationToken);
    public Task CreateAsync(Tag tag, CancellationToken cancellationToken);
    public Task UpdateAsync(Tag tag, CancellationToken cancellationToken);
    public Task DeleteAsync(Tag tag, CancellationToken cancellationToken);
    public Task<bool> ExistsByNameOrSlugAsync(
        string name,
        string slug,
        CancellationToken cancellationToken,
        Guid? excludedId = null);
}
