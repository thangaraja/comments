using System.Data.Entity;

namespace CommentSystems.Models
{
    public class CommentDbContext : DbContext
    {
        public CommentDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Comment> Comments { get; set; }
    }
}