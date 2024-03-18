using System.ComponentModel.DataAnnotations;
namespace Class_Exercise_1.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int ProjectCommentId { get; set; }
        [Required]
        [Display(Name = "Comment")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters. ")]
        public string? Content { get; set; }

        [Display(Name = "Posted Date")]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DatePosted {get;set;}

        //Foreign key for Project
        public int ProjectId { get; set; }

        // Navigation property to Project
        public Project? Project { get; set; }
    }
}
