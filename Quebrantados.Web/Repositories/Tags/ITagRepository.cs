using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Repositories.Tags;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TagListItem>> GetAllWithPostCountAsync(CancellationToken cancellationToken);
    Task CreateAsync(Tag tag, CancellationToken cancellationToken);
    Task UpdateAsync(Tag tag, CancellationToken cancellationToken);
    Task DeleteAsync(Tag tag, CancellationToken cancellationToken);
    Task<bool> ExistsByNameOrSlugAsync(
        string name,
        string slug,
        CancellationToken cancellationToken,
        Guid? excludedId = null);
}
