namespace Quebrantados.Web.DTOs.Tags;

public class TagListItem(Guid id, string name, string slug, int postCount)
{
    public Guid Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string Slug { get; init; } = slug;
    public int PostCount { get; init; } = postCount;
}