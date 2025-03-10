using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdultGamingForum.Data;
using AdultGamingForum.Models;
using Microsoft.AspNetCore.Authorization;

namespace AdultGamingForum.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ForumContext _context;

        public CommentsController(ForumContext context)
        {
            _context = context;
        }

        // GET: Comments (List All Comments)
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments.Include(c => c.Discussion).ToListAsync();
            return View(comments);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Discussion)
                .FirstOrDefaultAsync(m => m.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create?discussionId=1
        public IActionResult Create(int discussionId)
        {
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

                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }
            ViewBag.DiscussionId = comment.DiscussionId;
            return View(comment);
        }


        

        
    }
}
