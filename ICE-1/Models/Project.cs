// GitHub Repository Link: https://github.com/rocketbing/COMP2139-In-Class-Exercise-1.git
using System;
using System.ComponentModel.DataAnnotations;

namespace Class_Exercise_1.Models
{
	public class Project
	{
		//Primary Key
		public int ProjectId { get; set; }

		[Required]
		public required string Name { get; set; }

		//nullable
		public string? Description { get; set; }

		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

		public string? Status { get; set; }

	}
}

