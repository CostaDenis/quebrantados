using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Entities;

public class Category : Entity
{

    private readonly List<Post> _posts = [];

    private Category()
    {

    }

    public Category(CategoryName name, Slug slug)
    {
        Name = name;
        Slug = slug;
    }

    public CategoryName Name { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public IReadOnlyCollection<Post> Posts { get { return _posts.ToArray(); } }
}