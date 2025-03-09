using AdultGamingForum.Models;
using AdultGamingForum.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdultGamingForum.Data
{
    // use custom applicationUser
    public class ForumContext : IdentityDbContext<ApplicationUser>
    {
        public ForumContext(DbContextOptions<ForumContext> options)
            : base(options)
        {
        }

        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
