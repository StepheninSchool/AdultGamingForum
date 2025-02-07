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

namespace AdultGamingForum.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly ForumContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // Inject both ForumContext and IWebHostEnvironment into the constructor.
        public DiscussionsController(ForumContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Discussions/Index
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussions.ToListAsync();
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
        public async Task<IActionResult> Create(Discussion discussion, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Set the creation date.
                discussion.CreateDate = DateTime.Now;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Build the path to the uploads folder inside wwwroot.
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                    // Create the folder if it doesn't exist.
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    // Get the file name and combine with the uploads folder.
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    // Save the file to the specified location.
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // Update the Discussion model with the file name.
                    discussion.ImageFilename = fileName;
                }

                // Save the discussion to the database.
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }        

        // GET: Discussions/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var discussion = await _context.Discussions.FindAsync(id);
        //    if (discussion == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(discussion);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Discussion discussion, IFormFile ImageFile)
        //{
        //    if (id != discussion.DiscussionId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // If a new file is uploaded, process it.
        //            if (ImageFile != null && ImageFile.Length > 0)
        //            {
        //                // Build the path to the uploads folder inside wwwroot.
        //                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

        //                // Create the folder if it doesn't exist.
        //                if (!Directory.Exists(uploads))
        //                {
        //                    Directory.CreateDirectory(uploads);
        //                }

        //                var fileName = Path.GetFileName(ImageFile.FileName);
        //                var filePath = Path.Combine(uploads, fileName);

        //                // Save the file.
        //                using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await ImageFile.CopyToAsync(fileStream);
        //                }

        //                // Set the new file name in the discussion model.
        //                discussion.ImageFilename = fileName;
        //            }
        //            else
        //            {
        //                // No new file uploaded, so keep the existing image.
        //                // Retrieve the existing discussion from the database.
        //                var originalDiscussion = await _context.Discussions
        //                    .AsNoTracking()
        //                    .FirstOrDefaultAsync(d => d.DiscussionId == id);
        //                if (originalDiscussion != null)
        //                {
        //                    discussion.ImageFilename = originalDiscussion.ImageFilename;
        //                }
        //            }

        //            _context.Update(discussion);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DiscussionExists(discussion.DiscussionId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(discussion);
        //}

        // GET: Discussions/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var discussion = await _context.Discussions
        //        .FirstOrDefaultAsync(m => m.DiscussionId == id);
        //    if (discussion == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(discussion);
        //}

        //// POST: Discussions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var discussion = await _context.Discussions.FindAsync(id);
        //    if (discussion != null)
        //    {
        //        _context.Discussions.Remove(discussion);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.DiscussionId == id);
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
