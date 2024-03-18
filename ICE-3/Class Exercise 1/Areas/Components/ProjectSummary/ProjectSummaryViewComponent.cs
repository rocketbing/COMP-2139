using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Class_Exercise_1.Data;
using System.Linq;
using Class_Exercise_1.Areas.ProjectManagement.Models;

namespace Class_Exercise_1.Areas.ProjectManagement.Components.ProjectSummary
{
    /**
     * Lab 6 - This class is responsible for fetching the necessary data and passing it to the view
     */
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public ProjectSummaryViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var project = await _context.Projects
                                        .Include(p => p.Tasks)
                                        .FirstOrDefaultAsync(p => p.ProjectId == projectId);
            // Handle the case when the projects is not found
            if(project == null)
            {
                return Content("Project not found.");
            }
            return View(project);
        }
    }
}
