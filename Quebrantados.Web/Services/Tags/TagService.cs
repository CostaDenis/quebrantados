using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Exceptions.Services;
using Quebrantados.Web.Repositories.Tags;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Services.Tags;

public class TagService(ITagRepository tagRepository) : ITagService
{
    public async Task<TagOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByIdAsync(id, cancellationToken);

        if (tag is null)
            return null;

        return new TagOutput
        {
            Id = tag.Id,
            Name = tag.Name,
            Slug = tag.Slug
        };
    }

    public async Task<List<TagListItem>> GetAllAsync(CancellationToken cancellationToken)
        => await tagRepository.GetAllWithPostCountAsync(cancellationToken);

    public async Task CreateAsync(CreateTagInput input, CancellationToken cancellationToken)
    {
        var name = new TagName(input.Name);
        var slug = new Slug(input.Slug);

        var alreadyExists = await tagRepository.ExistsByNameOrSlugAsync(
            name.Value,
            slug.Value,
            cancellationToken);

        if (alreadyExists)
            throw new TagAlreadyExistsException();

        var tag = new Tag(name, slug);
        await tagRepository.CreateAsync(tag, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, EditTagInput input, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new TagNotFoundException();

        var name = new TagName(input.Name);
        var slug = new Slug(input.Slug);

        var alreadyExists = await tagRepository.ExistsByNameOrSlugAsync(
            name.Value,
            slug.Value,
            cancellationToken,
            id);

        if (alreadyExists)
            throw new TagAlreadyExistsException();

        tag.UpdateName(name);
        tag.UpdateSlug(slug);

        await tagRepository.UpdateAsync(tag, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new TagNotFoundException();

        await tagRepository.DeleteAsync(tag, cancellationToken);
    }

}
