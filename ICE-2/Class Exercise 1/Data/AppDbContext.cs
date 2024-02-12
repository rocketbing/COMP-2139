
//GitHub Repository Link: https://github.com/rocketbing/COMP2139-In-Class-Exercise-1.git
using System;
using Microsoft.EntityFrameworkCore;
using Class_Exercise_1.Models;
namespace Class_Exercise_1.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectTask> ProjectTasks { get; set; }

	}
}

