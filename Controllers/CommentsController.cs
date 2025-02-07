using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdultGamingForum.Data;
using AdultGamingForum.Models;

namespace AdultGamingForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ForumContext _context;

        public CommentsController(ForumContext context)
        {
            _context = context;
        }

        // GET: Comments/Create?discussionId=1
        public IActionResult Create(int discussionId)
        {
            // Pass the discussion ID to the view using ViewBag or ViewData.
            ViewBag.DiscussionId = discussionId;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreateDate = DateTime.Now;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to the "Get Discussion" page (Details view of DiscussionsController)
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }
            // If ModelState is invalid, reassign the DiscussionId and show the form again.
            ViewBag.DiscussionId = comment.DiscussionId;
            return View(comment);
        }
    }
}