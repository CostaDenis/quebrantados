using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quebrantados.Web.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Data.Mappings;

public class TagMapping : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid");

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasConversion(
                name => name.Value,
                value => new TagName(value))
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

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

        builder.HasMany(tag => tag.Posts)
            .WithMany(post => post.Tags)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                right => right
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("PostsTags");

                    join.HasKey("PostId", "TagId");
                }
            );

        builder.Navigation(tag => tag.Posts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}