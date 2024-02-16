using BlogService.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogService.Data.EntityConfigurations{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");
            
            builder.HasIndex(e => e.AuthenticationId).IsUnique();

            builder.Property(e => e.FirstName).HasMaxLength(255);
            builder.Property(e => e.LastName).HasMaxLength(255);
            builder.Property(e => e.AvatarPath).HasMaxLength(255);
        }
    }
}