using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Class_Exercise_1.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Class_Exercise_1.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var projects = new List<Project>()
            {
            new Project { ProjectId = 1, Name = "Project 1", Description = "This is my first Project"},
            new Project {ProjectId = 2, Name = "Project 2", Description = "This is my second Project"}

            };
            return View(projects);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = new Project { ProjectId = id, Name = "project " + id, Description = "Details of Project " + id };
            return View(project);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            return RedirectToAction("Index");
        }
    }
}

