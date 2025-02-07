using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdultGamingForum.Data;
using AdultGamingForum.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace AdultGamingForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ForumContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ForumContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Home/Index
        // Retrieves all discussions ordered by descending CreateDate.
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussions
                .OrderByDescending(d => d.CreateDate)
                .Include(d => d.Comments)
                .ToListAsync();
            return View(discussions);
        }

        // GET: Home/GetDiscussion/1
        // Retrieves a single discussion (with its comments) by ID.
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }
    }
}