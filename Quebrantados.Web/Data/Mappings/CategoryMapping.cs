using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quebrantados.Web.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Data.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid");

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasConversion(
                name => name.Value,
                value => new CategoryName(value))
            .HasColumnType("varchar")
            .HasMaxLength(60)
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

        builder.HasMany(category => category.Posts)
            .WithOne(post => post.Category)
            .HasForeignKey("Category_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(category => category.Posts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
