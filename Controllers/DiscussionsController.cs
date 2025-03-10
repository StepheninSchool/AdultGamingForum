using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdultGamingForum.Data;
using AdultGamingForum.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AdultGamingForum.Controllers
{
    [Authorize]
    public class DiscussionsController : Controller
    {
        private readonly ForumContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        // Inject both ForumContext and IWebHostEnvironment into the constructor.
        public DiscussionsController(ForumContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: Discussions/Index
        public async Task<IActionResult> Index()

        {
            var userId = _userManager.GetUserId(User);
            var discussions = await _context.Discussions
                .Where(d => d.ApplicationUserId == userId)
                .ToListAsync();
            return View(discussions);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Discussion discussion, IFormFile? ImageFile)
        {
            // Set the logged in user
            discussion.ApplicationUserId = _userManager.GetUserId(User);


            if (ModelState.IsValid)
            {
                discussion.CreateDate = DateTime.Now;

                _context.Add(discussion);
                await _context.SaveChangesAsync();

                // Handle image upload AFTER saving to get the DiscussionId
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    var fileExtension = Path.GetExtension(ImageFile.FileName);
                    var uniqueFileName = $"ADF_{discussion.DiscussionId}{fileExtension}";
                    var filePath = Path.Combine(imagesFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // Update the discussion with the image filename
                    discussion.ImageFilename = uniqueFileName;
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }




        //GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content")] Discussion discussion, IFormFile? ImageFile)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDiscussion = await _context.Discussions.AsNoTracking().FirstOrDefaultAsync(d => d.DiscussionId == id);
                    if (existingDiscussion == null)
                    {
                        return NotFound();
                    }

                    // Preserve original CreateDate
                    discussion.CreateDate = existingDiscussion.CreateDate;

                    // Handle image upload
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        var fileExtension = Path.GetExtension(ImageFile.FileName);
                        var uniqueFileName = $"ADF_{discussion.DiscussionId}{fileExtension}";
                        var filePath = Path.Combine(imagesFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        discussion.ImageFilename = uniqueFileName;
                    }
                    else
                    {
                        discussion.ImageFilename = existingDiscussion.ImageFilename;
                    }

                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Discussions.Any(d => d.DiscussionId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }




        //GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion != null)
            {
                _context.Discussions.Remove(discussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

    }
}
