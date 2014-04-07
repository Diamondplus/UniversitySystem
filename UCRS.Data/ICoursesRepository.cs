using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public interface ICoursesRepository
    {
        IQueryable<Course> Courses { get; }

        List<Course> GetRegisteredCourses(int studentId);

        List<Course> GetUnregisteredCourses(int studentId);

        bool IsCourseExist(string name);

        Course FindCourse(int? courseId);

        bool AddCourse(Course course);

        bool UpdateCourse(Course course);

        bool DeleteCourse(Course course);

        void Dispose();
        
        int CourseCount();
    }
}