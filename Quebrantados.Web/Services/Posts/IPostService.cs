using Quebrantados.Web.DTOs.Posts;

namespace Quebrantados.Web.Services.Posts;

public interface IPostService
{
    public Task<PostOutput> GetById(Guid id, CancellationToken cancellationToken);
    // public Task<List<>>
}