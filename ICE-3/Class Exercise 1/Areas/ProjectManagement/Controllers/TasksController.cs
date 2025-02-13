﻿using Microsoft.AspNetCore.Mvc;
using Class_Exercise_1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Class_Exercise_1.Areas.ProjectManagement.Models;

namespace Class_Exercise_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;

        public TasksController(AppDbContext context)
        {
            _db = context;
        }

        // GET: Tasks
        [HttpGet("Index/{projectId:int}")]
        public async Task<IActionResult> Index(int? projectId)
        {
            //var tasks = _db.ProjectTasks
            //                    .Where(t => t.ProjectId == projectId)
            //                    .ToList();
            //ViewBag.ProjectId = projectId;     // Store projectId in ViewBag
            //return View(tasks);
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId.Value);
            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId;     // Store projectId in ViewBag
            return View(tasks);
        }

        // GET: Tasks/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            
            var task = await _db.ProjectTasks
                            .Include(t => t.Project) // Include related project data
                            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpGet("Create/{projectId:int}")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound(); // Or handle appropriately if project doesn't exist
            }

            var task = new ProjectTask
            {
                ProjectId = projectId
            };

            return View(task);
        }



        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _db.ProjectTasks.AddAsync(task);
                await _db.SaveChangesAsync();
                // Redirect to the Index action with the projectId of the created task
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            // Async call to retrieve projects for SelectList
            var projects = await _db.Projects.ToListAsync();

            // Repopulate the Projects SelectList if returning to the form
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _db.ProjectTasks
                                .Include(t => t.Project) // Include related project data
                                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }
            var projects = await _db.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);   // not needed
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            var projects = await _db.Projects.ToListAsync();
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.ProjectTasks
                                .Include(t => t.Project) // Include related project data
                                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectTaskId)
        {
            var task = await _db.ProjectTasks.FindAsync(ProjectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            return NotFound();
        }
        //Lab 5 - Search ProjectTasks
        // GET: Tasks/Search/{projectId}/{searchString?}
        [HttpGet("Search")]
        public async Task<IActionResult> Search(int? projectId, string searchString)
        {
            // var tasksQuery = _db.ProjectTasks.Where(t => t.ProjectId == projectId);
            var taskQuery = _db.ProjectTasks.AsQueryable();
            bool searchPerformed = !String.IsNullOrEmpty(searchString);

            // If a projectId is provided, apply it as a filter
            if(projectId.HasValue)
            {
                taskQuery = taskQuery.Where(t => t.ProjectId == projectId.Value);
            }

            // Apply the search string filter if it is not null or empty
            if(!searchPerformed)
            {
                taskQuery = taskQuery.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
            }

            // Execute the query
            var tasks = await taskQuery.ToListAsync();
            
            // Depending on your UI, you might want to handle the ViewBag differently if no specific project is targeted.
            ViewBag.ProjectId = projectId; // To keep track of the current project
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", tasks); // Reuse the Index view to display results
        }
    }
}