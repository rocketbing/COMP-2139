using Class_Exercise_1.Areas.ProjectManagement.Models;
using Class_Exercise_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Class_Exercise_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectCommentController : Controller
    {


        private readonly AppDbContext _context;

        public ProjectCommentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProjectManagement/ProjectComment/GetComments/{projectId}
        [HttpGet]
        public async Task<IActionResult> GetComments(int projectId)
        {
            var comments = await _context.ProjectComments
                                         .Where(c => c.ProjectId == projectId)
                                         .OrderByDescending(c => c.DatePosted)
                                         .ToListAsync();

            return Json(comments);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.DatePosted = DateTime.Now; // Set the current time as the posting time
                _context.ProjectComments.Add(comment);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Comment added successfully." });
            }

            // Log ModelState errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid comment data.", error = errors });
        }
    }
}
