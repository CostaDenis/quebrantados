using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Exceptions.Services;
using Quebrantados.Web.Repositories.Categories;
using Quebrantados.Web.Repositories.Posts;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Services.Posts;

public class PostService(IPostRepository postRepository,
    ICategoryRepository categoryRepository) : IPostService
{

    public async Task<PostOutput?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken);

        if (post is null)
            return null;

        return new PostOutput(
            post.Title, post.Slug, post.Summary?.Value, post.Body,
            post.CreatedAt, post.LastUpdateDate, post.Category.Name.Value);
    }

    public async Task<List<PostListItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        var posts = await postRepository.GetAllAsync(cancellationToken);

        return posts.Select(post => new PostListItem
            (post.Title, post.Category.Name.Value,
                post.Status.ToString(), post.LastUpdateDate
            )).ToList();
    }

    public async Task CreateAsync(CreatePostInput input, CancellationToken cancellationToken)
    {
        var title = new Title(input.Title);
        var slug = new Slug(input.Slug);
        var alreadyExists = await postRepository
            .ExistsTitleOrSlugAsync(title.Value, slug.Value, cancellationToken);

        if (alreadyExists)
            throw new PostAlreadyExistsException();

        var category = await categoryRepository.GetByIdAsync(input.CategoryId, cancellationToken)
            ?? throw new CategoryNotFoundException();

        var post = new Post(title, slug,
            input.Summary is null ? null : new Summary(input.Summary),
            new Body(input.Body), category);

        await postRepository.CreateAsync(post, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, EditPostInput input, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new PostNotFoundException();

        var title = new Title(input.Title);
        var slug = new Slug(input.Slug);
        var alreadyExists = await postRepository
            .ExistsTitleOrSlugAsync(title.Value, slug.Value, cancellationToken, post.Id);

        if (alreadyExists)
            throw new PostAlreadyExistsException();

        var category = await categoryRepository.GetByIdAsync(input.CategoryId, cancellationToken)
            ?? throw new CategoryNotFoundException();

        post.UpdateTitle(title);
        post.UpdateSlug(slug);
        post.UpdateSummary(
        input.Summary is null ? null : new Summary(input.Summary));
        post.UpdateBody(new Body(input.Body));
        post.UpdateCategory(category);
        post.UpdateLastUpdateDate(DateTime.UtcNow);

        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new PostNotFoundException();

        await postRepository.DeleteAsync(post, cancellationToken);
    }

    public async Task PublishAsync(Guid id, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new PostNotFoundException();

        post.Publish(DateTime.UtcNow);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task MoveToDraftAsync(Guid id, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new PostNotFoundException();

        post.MoveToDraft(DateTime.UtcNow);
        await postRepository.UpdateAsync(post, cancellationToken);
    }
}