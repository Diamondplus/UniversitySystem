using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UCRS.Model
{
    public class StudentInCourse
    {
        [Key, ForeignKey("Student"), Column(Order = 0)]
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        [Key, ForeignKey("Course"), Column(Order = 1)]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}