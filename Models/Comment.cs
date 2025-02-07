namespace AdultGamingForum.Models
{
    public class Comment
    {
        //Primary Key
        public int CommentId { get; set; }
        //Comment's text content
        public string Content { get; set; } = string.Empty;
        //The date and time when the comment was created
        public DateTime CreateDate { get; set; }
        //Foreign key to link comment with a discussion
        public int DiscussionId { get; set; }
        //Navigation property: The discussion this comment belongs to.
        public Discussion? Discussion { get; set; }
    }
}
