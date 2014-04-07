using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Models
{
    public class MyCoursesViewModel
    {
        public Student Student { get; set; }
        public List<Course> RegList { get; set; }
        public List<Course> UnregList { get; set; }

        public MyCoursesViewModel()
        {
            this.Student = new Student();
            this.RegList = new List<Course>();
            this.UnregList = new List<Course>();
        }

        public MyCoursesViewModel( Student student, List<Course> regList, List<Course> unregList  )
        {
            this.Student = student;
            this.RegList = regList;
            this.UnregList = unregList;
        }
    }
}