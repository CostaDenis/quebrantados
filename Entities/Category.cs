using quebrantados.ValueObjects;

namespace quebrantados.Entities;

public class Category(string name, Slug slug)
    : Entity
{
    private readonly List<Post> _posts = [];

    public string Name { get; private set; } = name;
    public Slug Slug { get; private set; } = slug;
    public IReadOnlyCollection<Post> Posts { get { return _posts.ToArray(); } }
}