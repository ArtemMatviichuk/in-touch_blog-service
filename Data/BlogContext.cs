using Microsoft.EntityFrameworkCore;

namespace BlogService.Data;
public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> opt)
        : base(opt)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}