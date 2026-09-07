using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Exceptions.Services;
using Quebrantados.Web.Repositories.Categories;
using Quebrantados.Web.Repositories.Posts;
using Quebrantados.Web.Repositories.Tags;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Services.Posts;

public class PostService(IPostRepository postRepository,
    ICategoryRepository categoryRepository, ITagRepository tagRepository) : IPostService
{

    public async Task<PostOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken);

        if (post is null)
            return null;

        return new PostOutput(post.Id, post.Title, post.Slug,
            post.Summary?.Value, post.Body, post.CreatedAt,
            post.LastUpdateDate, post.Category.Id, post.Category.Name.Value,
            post.Status, post.PublishedAt, post.Tags.Select(tag => tag.Id).ToList());
    }

    public async Task<List<PostListItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        var posts = await postRepository.GetAllAsync(cancellationToken);

        return posts.Select(post => new PostListItem
            (post.Id, post.Title, post.Slug, post.Category.Name.Value,
                post.Status, post.LastUpdateDate
            )).ToList();
    }

    public async Task CreateAsync(CreatePostInput input, bool publish, CancellationToken cancellationToken)
    {
        var title = new Title(input.Title);
        var slug = new Slug(input.Slug);
        var alreadyExists = await postRepository
            .ExistsTitleOrSlugAsync(title.Value, slug.Value, cancellationToken);

        if (alreadyExists)
            throw new PostAlreadyExistsException();

        var category = await categoryRepository.GetByIdAsync(input.CategoryId, cancellationToken)
            ?? throw new CategoryNotFoundException();

        var summary = string.IsNullOrWhiteSpace(input.Summary)
            ? null
            : new Summary(input.Summary);

        var post = new Post(title, slug, summary, new Body(input.Body), category);

        var requestedTagIds = input.TagIds.Distinct().ToList();

        var tags = await tagRepository.GetByIdsAsync(requestedTagIds, cancellationToken);

        if (tags.Count != requestedTagIds.Count)
            throw new TagNotFoundException();

        post.ReplaceTags(tags);

        if (publish)
            post.Publish(DateTime.UtcNow);

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

        var requestedTagIds = input.TagIds.Distinct().ToList();

        var tags = await tagRepository.GetByIdsAsync(requestedTagIds, cancellationToken);

        if (tags.Count != requestedTagIds.Count)
            throw new TagNotFoundException();

        post.ReplaceTags(tags);

        var summary = string.IsNullOrWhiteSpace(input.Summary)
            ? null
            : new Summary(input.Summary);

        post.UpdateTitle(title);
        post.UpdateSlug(slug);
        post.UpdateSummary(summary);
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
