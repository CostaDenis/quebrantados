using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Entities;

public class Tag : Entity
{
    private readonly List<Post> _posts = [];

    private Tag()
    {

    }

    public Tag(TagName name, Slug slug)
    {
        Name = name;
        Slug = slug;
    }

    public TagName Name { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public IReadOnlyCollection<Post> Posts { get { return _posts.ToArray(); } }

    public void UpdateName(TagName name)
        => Name = name;

    public void UpdateSlug(Slug slug)
        => Slug = slug;
}