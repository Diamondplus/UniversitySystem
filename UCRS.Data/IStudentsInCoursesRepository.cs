using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public interface IStudentsInCoursesRepository
    {
        IQueryable<StudentInCourse> StudentsInCourses { get; }

        bool IsStudentInCourseExist(int studentId, int courseId);

        bool AddStudentInCourse(int studentId, int courseId);

        bool DeleteStudentInCourse(int studentId, int courseId);

        bool DeleteAllMyCourses(int studentId);

        int NumberOfStudentsInCourse(int courseId);

        int NumberOfCoursesStudent(int studentId);

        int StudentsInCoursesCount();
    }
}