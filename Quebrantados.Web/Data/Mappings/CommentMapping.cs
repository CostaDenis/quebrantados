using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quebrantados.Web.Entities;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Data.Mappings;

public class CommentMapping : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.PostId)
            .HasColumnName("PostId")
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasOne(comment => comment.Post)
            .WithMany()
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.AuthorName)
            .HasColumnName("AuthorName")
            .HasConversion(
                name => name.Value,
                value => new CommentAuthorName(value))
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(x => x.AuthorEmail)
            .HasColumnName("AuthorEmail")
            .HasConversion(
                email => email.Value,
                value => new EmailAddress(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnName("Content")
            .HasConversion(
                content => content.Value,
                value => new CommentContent(value))
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestampz")
            .IsRequired();
    }
}