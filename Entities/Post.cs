using quebrantados.ValueObjects;

namespace quebrantados.Entities;

public class Post(Title title, Slug slug, Summary? summary, Body body, DateTime LastUpdateDate, Category category) : Entity
{
    public Title Title { get; private set; } = title;
    public Slug Slug { get; private set; } = slug;
    public Summary? Summary { get; private set; } = summary;
    public Body Body { get; private set; } = body;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; private set; } = LastUpdateDate;
    public Category Category { get; private set; } = category;
}