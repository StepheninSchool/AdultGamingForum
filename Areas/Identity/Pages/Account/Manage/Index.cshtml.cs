// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AdultGamingForum.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdultGamingForum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment; // Add IWebHostEnvironment
        private readonly ILogger<IndexModel> _logger; // Add ILogger

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment, // Add IWebHostEnvironment
            ILogger<IndexModel> logger // Add ILogger
        )
        
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// BEGIN CHANGES
            /// </summary>
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; } = string.Empty;

            [Display(Name = "Bio")]
            public string Bio { get; set; } = string.Empty;

            [Display(Name = "Location")]
            public string Location { get; set; } = string.Empty;

            [Display(Name = "Favorite Game")]
            public string FavoriteGame { get; set; } = string.Empty;

            [Display(Name = "Profile Image")]
            public IFormFile? ImageFile { get; set; }

            public string? ImageFilename { get; set; }
            /// END CHANGES
            ///
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                //
                //BEGIN CHANGES
                //
                Name = user.Name,
                Bio = user.Bio,
                Location = user.Location,
                FavoriteGame = user.FavoriteGame,
                ImageFilename = user.ImageFilename
                //
                //END CHANGES
                //
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            //
            // BEGIN CHANGES : ApplicationUser custom fields 
            //
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }
            if (Input.Bio != user.Bio)
            {
                user.Bio = Input.Bio;
            }
            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
            }
            if (Input.FavoriteGame != user.FavoriteGame)
            {
                user.FavoriteGame = Input.FavoriteGame;
            }
            if (Input.ImageFile != null && Input.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(fileStream);
                }

                user.ImageFilename = fileName;
            }


            await _userManager.UpdateAsync(user);
            //
            // END CHANGES : ApplicationUser custom fields 3/09/25 6:20pm
            //
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
