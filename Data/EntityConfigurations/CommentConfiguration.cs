using BlogService.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogService.Data.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.Property(e => e.Text).IsRequired();

            builder.HasOne(e => e.Post).WithMany(e => e.Comments).HasForeignKey(e => e.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Parent).WithMany(e => e.Comments).HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
