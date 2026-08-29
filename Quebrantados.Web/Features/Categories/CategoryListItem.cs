namespace Quebrantados.Web.Features.Categories;

public class CategoryListItem(Guid id, string name, string slug, int postCount)
{
    public Guid Id { get; init; } = id;
    public string Name { get; set; } = name;
    public string Slug { get; init; } = slug;
    public int PostCount { get; init; } = postCount;
}