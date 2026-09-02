using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Exceptions.Services;
using Quebrantados.Web.Repositories.Categories;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Services.Categories;

public class CategoryService(ICategoryRepository categoryRepository)
    : ICategoryService
{

    public async Task<CategoryOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            return null;

        return new CategoryOutput(category.Name, category.Slug);
    }

    public async Task<List<CategoryListItem>> GetAllAsync(CancellationToken cancellationToken)
        => await categoryRepository.GetAllWithPostCountAsync(cancellationToken);

    public async Task CreateAsync(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var name = new CategoryName(input.Name);
        var slug = new Slug(input.Slug);
        var alreadyExists = await categoryRepository
            .ExistsByNameOrSlugAsync(name.Value, slug.Value, cancellationToken);

        if (alreadyExists)
            throw new CategoryAlreadyExistsException();

        var category = new Category(new CategoryName(input.Name), new Slug(input.Slug));
        await categoryRepository.CreateAsync(category, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, EditCategoryInput input, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new CategoryNotFoundException();

        var name = new CategoryName(input.Name);
        var slug = new Slug(input.Slug);
        var alreadyExists = await categoryRepository
            .ExistsByNameOrSlugAsync(name.Value, slug.Value, cancellationToken, category.Id);

        if (alreadyExists)
            throw new CategoryAlreadyExistsException();

        category.UpdateName(new CategoryName(input.Name));
        category.UpdateSlug(new Slug(input.Slug));

        await categoryRepository.UpdateAsync(category, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new CategoryNotFoundException();

        var hasPosts = await categoryRepository.HasPostsAsync(id, cancellationToken);
        if (hasPosts)
            throw new CategoryHasPostsException();

        await categoryRepository.DeleteAsync(category, cancellationToken);
    }
}