using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quebrantados.Entities;
using quebrantados.ValueObjects;

namespace quebrantados.Data.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .HasMaxLength(60)
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

        builder.HasMany(category => category.Posts)
            .WithOne(post => post.Category)
            .HasForeignKey("category_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(category => category.Posts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
