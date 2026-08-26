using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quebrantados.Entities;
using quebrantados.ValueObjects;

namespace quebrantados.Data.Mappings;

public class PostMapping : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasConversion(
                title => title.Value,
                value => new Title(value))
            .HasColumnType("varchar")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasColumnName("slug")
            .HasConversion(
                slug => slug.Value,
                value => new Slug(value))
            .HasColumnType("varchar")
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.Property(x => x.Summary)
            .HasColumnName("summary")
            .HasConversion(
                summary => summary!.Value,
                value => new Summary(value))
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.Body)
            .HasColumnName("body")
            .HasConversion(
                body => body.Value,
                value => new Body(value))
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestampz")
            .IsRequired();

        builder.Property(x => x.LastUpdateDate)
            .HasColumnName("last_update_date")
            .HasColumnType("timestampz")
            .IsRequired();

        builder.Navigation(post => post.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
