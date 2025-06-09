using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshoManager.Data;
using WorkshoManager.Models;
using WorkshoManager.Data;
using WorkshoManager.Models;

namespace WorkshoManager.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int orderId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Komentarz nie może być pusty.";
                return RedirectToAction("Details", "Orders", new { id = orderId });
            }

            var comment = new Comment
            {
                Content = content,
                CreatedAt = DateTime.Now,
                AuthorId = _userManager.GetUserId(User),
                OrderId = orderId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Orders", new { id = orderId });
        }
    }
}