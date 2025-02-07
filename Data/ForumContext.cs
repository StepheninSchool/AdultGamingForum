using AdultGamingForum.Models;
using AdultGamingForum.Data;
using Microsoft.EntityFrameworkCore;

namespace AdultGamingForum.Data
{
    public class ForumContext : DbContext
    {
        public ForumContext(DbContextOptions<ForumContext> options)
            : base(options)
        {
        }

        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
