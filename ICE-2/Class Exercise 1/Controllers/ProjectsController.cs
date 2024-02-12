using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Class_Exercise_1.Data;
using Class_Exercise_1.Models;
using Microsoft.AspNetCore.Mvc;
using Class_Exercise_1.Controllers;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Class_Exercise_1.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;

        public ProjectsController(AppDbContext db)
        {
            _db = db;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
           
            return View(_db.Projects.ToList());
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if(project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if(ModelState.IsValid)
            {
                _db.Projects.Add(project);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }
        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if(project == null)
            {
                return NotFound();

            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
        {
            if(id != project.ProjectId)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        private bool ProjectExists(int id)
        {
            return _db.Projects.Any(e => e.ProjectId == id);
        }
        public IActionResult Delete (int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if(project == null)
            {
                return NotFound();

            }
            return View(project);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProjectId)
        {
            var project = _db.Projects.Find(ProjectId);
            if(project != null)
            {
                _db.Projects.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}

