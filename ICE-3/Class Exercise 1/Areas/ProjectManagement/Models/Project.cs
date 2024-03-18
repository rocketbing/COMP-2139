
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Class_Exercise_1.Areas.ProjectManagement.Models
{
    public class Project
    {
        //Primary Key
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Project Name")]
        public required string Name { get; set; }

        //nullable
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)] 
        public DateTime EndDate { get; set; }

        public string? Status { get; set; }
        public List<ProjectTask>? Tasks { get; set; }

    }
}

