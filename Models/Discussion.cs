using AdultGamingForum.Data;
#nullable disable

namespace AdultGamingForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; } // Primary Key
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ImageFilename { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Foreign key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; } // nullable!!!

    }
}