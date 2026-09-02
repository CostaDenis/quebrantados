using Quebrantados.Web.DTOs.Tags;

namespace Quebrantados.Web.Services.Tags;

public interface ITagService
{
    public Task<TagOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<TagListItem>> GetAllAsync(CancellationToken cancellationToken);
    public Task CreateAsync(CreateTagInput input, CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, EditTagInput input, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}