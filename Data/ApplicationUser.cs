using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;


namespace AdultGamingForum.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]// property is included in download of personal data.
        public string Name { get; set; } = string.Empty;

        [PersonalData]
        public string? Bio { get; set; } = string.Empty;

        [PersonalData]
        public string? Location { get; set; } = string.Empty;

        [PersonalData]
        public string FavoriteGame { get; set; } = string.Empty;

        [PersonalData]
        public string? ImageFilename { get; set; } = string.Empty;
    }
}
