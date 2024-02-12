using Microsoft.AspNetCore.Mvc;
using Class_Exercise_1.Data;
using Class_Exercise_1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Class_Exercise_1.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;
        public TasksController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks.Where(t => t.ProjectId == projectId).ToList();
            ViewBag.ProjectId = projectId; //Store projectId in ViewBag
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }
        public IActionResult Details(int id)
        {
            var task = _db.ProjectTasks
                .Include(t => t.Project) // Include related project data
                .FirstOrDefault(t => t.ProjectTaskId == id);
            if(task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if(project == null)
            {
                return NotFound(); // Or handle appropriately if project doesn't exist
            }
            var task = new ProjectTask { ProjectId = projectId };
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if(ModelState.IsValid)
            {
                _db.ProjectTasks.Add(task);
                _db.SaveChanges();
                // Redirect to the Index action with the projectId of the created task
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            // Repopulate the Projects SelectList if returning to the form
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var task = _db.ProjectTasks
                        .Include(t => t.Project) // Include related project data
                        .FirstOrDefault(t => t.ProjectTaskId == id);

            if(task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if(id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                _db.ProjectTasks.Update(task);
                _db.SaveChanges();
                return RedirectToAction("Index", new { ProjectId = task.ProjectId });
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _db.ProjectTasks
                            .Include(t => t.Project) // Include related project data
                            .FirstOrDefault(t => t.ProjectTaskId == id);
            if(task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProjectTaskId)
        {
            var task = _db.ProjectTasks.Find(ProjectTaskId);
            if(task != null)
            {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return NotFound();
        }
    }
}
