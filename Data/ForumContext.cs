using AdultGamingForum.Models;
using AdultGamingForum.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdultGamingForum.Data
{
    public class ForumContext : IdentityDbContext
    {
        public ForumContext(DbContextOptions<ForumContext> options)
            : base(options)
        {
        }

        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
