using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace AdultGamingForum.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string Name { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? ImageFileName { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
