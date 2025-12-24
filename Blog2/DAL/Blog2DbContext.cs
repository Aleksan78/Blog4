using Blog2.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog2.DAL
{
    public class Blog2DbContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public Blog2DbContext(DbContextOptions<Blog2DbContext> options) : base(options)
        {
            //Database.Migrate(); перенес в программ
        }
    }
}
