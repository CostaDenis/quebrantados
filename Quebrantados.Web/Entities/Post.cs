using Quebrantados.Web.Enums;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Entities;

public class Post : Entity
{
    private readonly List<Tag> _tags = [];

    private Post()
    {

    }

    public Post(Title title, Slug slug, Summary? summary, Body body, DateTime lastUpdateDate, Category category)
    {
        Title = title;
        Slug = slug;
        Summary = summary;
        Body = body;
        LastUpdateDate = lastUpdateDate;
        Category = category;
    }

    public Title Title { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public Summary? Summary { get; private set; } = null!;
    public Body Body { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; private set; }
    public Category Category { get; private set; } = null!;
    public EPostStatus Status { get; private set; } = EPostStatus.Draft; //todo post começa como rascunho
    public DateTime? PublishedAt { get; private set; }
    public IReadOnlyCollection<Tag> Tags { get { return _tags.ToArray(); } }

    public void Publish(DateTime publicationDate)
    {
        Status = EPostStatus.Published;
        PublishedAt ??= publicationDate;
        LastUpdateDate = publicationDate;
    }

    public void MoveToDraft(DateTime updateDate)
    {
        Status = EPostStatus.Draft;
        LastUpdateDate = updateDate;
    }
}