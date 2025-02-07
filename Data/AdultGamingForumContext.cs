using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdultGamingForum.Models;

namespace AdultGamingForum.Data
{
    public class AdultGamingForumContext : DbContext
    {
        public AdultGamingForumContext (DbContextOptions<AdultGamingForumContext> options)
            : base(options)
        {
        }

        public DbSet<AdultGamingForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<AdultGamingForum.Models.Comment> Comment { get; set; } = default!;
    }
}
