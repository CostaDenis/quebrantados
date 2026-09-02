using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quebrantados.Web.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Data.Mappings;

public class PostMapping : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid");

        builder.Property(x => x.Title)
            .HasColumnName("Title")
            .HasConversion(
                title => title.Value,
                value => new Title(value))
            .HasColumnType("varchar")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasColumnName("Slug")
            .HasConversion(
                slug => slug.Value,
                value => new Slug(value))
            .HasColumnType("varchar")
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.Property(x => x.Summary)
            .HasColumnName("Summary")
            .HasConversion(
                summary => summary!.Value,
                value => new Summary(value))
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.Body)
            .HasColumnName("Body")
            .HasConversion(
                body => body.Value,
                value => new Body(value))
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestampz")
            .IsRequired();

        builder.Property(x => x.LastUpdateDate)
            .HasColumnName("LastUpdateDate")
            .HasColumnType("timestampz")
            .IsRequired();

        builder.Navigation(post => post.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.PublishedAt)
            .HasColumnName("PublishedAt")
            .HasColumnType("timestampz")
            .IsRequired(false);
    }
}
