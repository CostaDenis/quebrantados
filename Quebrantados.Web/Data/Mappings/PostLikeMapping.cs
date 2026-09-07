using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quebrantados.Web.Entities;

namespace Quebrantados.Web.Data.Mappings;

public class PostLikeMapping : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.ToTable("PostLikes");

        builder.HasKey(postLike => postLike.Id);

        builder.Property(postLike => postLike.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(postLike => postLike.PostId)
            .HasColumnName("PostId")
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasOne(postLike => postLike.Post)
            .WithMany()
            .HasForeignKey(postLike => postLike.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(postLike => postLike.VisitorId)
            .HasColumnName("VisitorId")
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasIndex(postLike => new
        {
            postLike.PostId,
            postLike.VisitorId
        })
        .IsUnique();
    }
}