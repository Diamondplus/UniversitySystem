using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public class StudentsInCoursesRepository : IStudentsInCoursesRepository
    {
        private UniversityContext db = new UniversityContext();

        public IQueryable<StudentInCourse> StudentsInCourses
        {
            get { return db.StudentsInCourses.AsQueryable(); }
        }

        public bool IsStudentInCourseExist(int studentId, int courseId)
        {
            StudentInCourse dbEntry = db.StudentsInCourses.FirstOrDefault(sic => (sic.StudentId == studentId && sic.CourseId == courseId));
            if (dbEntry == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AddStudentInCourse(int studentId, int courseId)
        {
            StudentInCourse dbEntry = db.StudentsInCourses.FirstOrDefault(sic => (sic.StudentId == studentId && sic.CourseId == courseId));
            if (dbEntry == null)
            {
                try
                {
                    var newStudentInCourse = db.StudentsInCourses.Create();

                    newStudentInCourse.StudentId = studentId;
                    newStudentInCourse.CourseId = courseId;

                    db.StudentsInCourses.Add(newStudentInCourse);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                        String.Format(" UCRS.StudentInCourses NEW StudentInCourse ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool DeleteStudentInCourse(int studentId, int courseId)
        {
            StudentInCourse dbEntry = db.StudentsInCourses.FirstOrDefault(sic => (sic.StudentId == studentId && sic.CourseId == courseId));
            if (dbEntry != null)
            {
                try
                {
                    string queryString = "DELETE FROM StudentInCourses WHERE ( StudentId = " + studentId + " AND CourseId = "+ courseId + " )";
                    var objCtxDb = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    objCtxDb.ExecuteStoreCommand(queryString);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                            String.Format(" UCRS.StudentInCourses DELETE StudentInCourse ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool DeleteAllMyCourses( int studentId)
        {
            StudentInCourse dbEntry = db.StudentsInCourses.FirstOrDefault(sic => sic.StudentId == studentId);
            if (dbEntry != null)
            {
                try
                {
                    string queryString = "DELETE FROM StudentInCourses WHERE ( StudentId = " + studentId + " )";
                    var objCtxDb = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    objCtxDb.ExecuteStoreCommand(queryString);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                            String.Format(" UCRS.StudentInCourses DELETE All MY Courses ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public int NumberOfStudentsInCourse(int courseId)
        {
            int numberOfStudentsInCourse = db.StudentsInCourses.Count(sic => sic.CourseId == courseId);
            return numberOfStudentsInCourse;
        }

        public int NumberOfCoursesStudent(int studentId)
        {
            int numberOfCoursesStudent = db.StudentsInCourses.Count(sic => sic.StudentId == studentId);
            return numberOfCoursesStudent;
        }

        public int StudentsInCoursesCount()
        {
            int studentsInCoursesCount = db.StudentsInCourses.Count();
            return studentsInCoursesCount;
        }
    }
}