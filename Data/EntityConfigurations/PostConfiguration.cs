using BlogService.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogService.Data.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Text).IsRequired();

            builder.HasOne(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
