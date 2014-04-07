using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UCRS.Model
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(AutoGenerateField = true)]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course Name is required")]
        [Display(Name = "Course Name", Description = "Input Course Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Course Name must be between 2 and 50 characters long.")]
        public string Name { get; set; }
    }
}