using System.Diagnostics;
using AdultGamingForum.Data;
using AdultGamingForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdultGamingForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumContext _context;

        // Inject both ILogger and ForumContext via the constructor.
        public HomeController(ILogger<HomeController> logger, ForumContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Home/Index
        // This action retrieves all discussions (posts) ordered by creation date in descending order.
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Discussions
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();
            return View(posts);
        }

        // GET: Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
    }
}