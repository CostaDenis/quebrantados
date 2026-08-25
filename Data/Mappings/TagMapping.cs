using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quebrantados.Entities;
using quebrantados.ValueObjects;

namespace quebrantados.Data.Mappings;

public class TagMapping : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasConversion(
                name => name.Value,
                value => new TagName(value))
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

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

        builder.HasMany(tag => tag.Posts)
            .WithMany(post => post.Tags)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                right => right
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("post_id")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("tag_id")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("posts_tags");

                    join.HasKey("post_id", "tag_id");
                }
            );

        builder.Navigation(tag => tag.Posts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}