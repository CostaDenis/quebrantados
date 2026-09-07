using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions.Services;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Entities;

public class Post : Entity
{
    private readonly List<Tag> _tags = [];

    private Post()
    {
    }

    public Post(Title title, Slug slug, Summary? summary,
        Body body, Category category)
    {
        Title = title;
        Slug = slug;
        Summary = summary;
        Body = body;
        Category = category;
    }

    public Title Title { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public Summary? Summary { get; private set; } = null!;
    public Body Body { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; private set; } = DateTime.UtcNow;
    public Category Category { get; private set; } = null!;
    public EPostStatus Status { get; private set; } = EPostStatus.Draft; //todos os posts começam como rascunho
    public DateTime? PublishedAt { get; private set; }
    public IReadOnlyCollection<Tag> Tags { get { return _tags.ToArray(); } }

    public void UpdateTitle(Title title)
        => Title = title;

    public void UpdateSlug(Slug slug)
        => Slug = slug;

    public void UpdateSummary(Summary? summary)
        => Summary = summary;

    public void UpdateBody(Body body)
        => Body = body;

    public void UpdateLastUpdateDate(DateTime lastUpateDate)
        => LastUpdateDate = lastUpateDate;

    public void UpdateCategory(Category category)
        => Category = category;

    public void Publish(DateTime publicationDate)
    {
        if (Status == EPostStatus.Published)
            throw new PostAlreadyPublishedException();

        Status = EPostStatus.Published;
        PublishedAt ??= publicationDate;
        LastUpdateDate = publicationDate;
    }

    public void MoveToDraft(DateTime updateDate)
    {
        if (Status == EPostStatus.Draft)
            throw new PostAlreadyInDraftException();

        Status = EPostStatus.Draft;
        LastUpdateDate = updateDate;
    }

    public void AddTag(Tag tag)
    {
        if (_tags.All(existingTag => existingTag.Id != tag.Id))
            _tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
        => _tags.RemoveAll(existingTag => existingTag.Id == tag.Id);

    public void ReplaceTags(IEnumerable<Tag> tags)
    {
        _tags.Clear();

        foreach (var tag in tags.DistinctBy(tag => tag.Id))
            _tags.Add(tag);
    }
}
