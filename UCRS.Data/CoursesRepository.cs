using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public class CoursesRepository : ICoursesRepository
    {
        private UniversityContext db = new UniversityContext();

        public IQueryable<Course> Courses
        {
            get { return db.Courses.AsQueryable(); }
        }

        public List<Course> GetRegisteredCourses(int studentId)
        {
            var q = ( from c in db.Courses
                     join sic in db.StudentsInCourses on c.CourseId equals sic.CourseId
                     where sic.StudentId == studentId
                     select new { CourseId = c.CourseId, Name = c.Name }).ToList();

            return q.Select(item => new Course() {CourseId = item.CourseId, Name = item.Name}).ToList();
        }

        public List<Course> GetUnregisteredCourses(int studentId)
        {
            List<Course> allCourses = db.Courses.ToList();
            List<Course> unregCourses = new List<Course>();
            foreach (Course course in allCourses)
            {
                var dbEntry = db.StudentsInCourses.FirstOrDefault(sic => (sic.StudentId == studentId && sic.CourseId == course.CourseId));
                if (dbEntry == null)
                {
                    unregCourses.Add(course);
                }                
            }
            return unregCourses;
        }

        public bool IsCourseExist(string name)
        {
            Course dbEntry = db.Courses.FirstOrDefault(c => c.Name == name);
            if (dbEntry == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Course FindCourse(int? courseId)
        {
            Course dbEntry = db.Courses.FirstOrDefault(c => c.CourseId == courseId);
            return dbEntry;
        }

        public bool AddCourse(Course course)
        {
            Course dbEntry = db.Courses.FirstOrDefault(c => c.Name == course.Name.Trim());
            if (dbEntry == null)
            {
                try
                {
                    var newCourse = db.Courses.Create();
                    newCourse.Name = course.Name.Trim();
                    db.Courses.Add(newCourse);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                        String.Format(" UCRS.Courses ADD COURSE ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool UpdateCourse(Course course)
        {
            Course dbEntry = db.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
            if (dbEntry != null )
            {
                try
                {
                    string queryString = "UPDATE Courses SET Name='" + course.Name.Trim() + "' WHERE (CourseId = '" + course.CourseId + "')";
                    var objCtxDb = ((System.Data.Entity.Infrastructure.IObjectContextAdapter) db).ObjectContext;
                    objCtxDb.ExecuteStoreCommand(queryString);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                        String.Format(" UCRS.Courses UPDATE COURSE ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool DeleteCourse(Course course)
        {
            Course dbEntry = db.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
            if (dbEntry != null)
            {
                try
                {
                    string queryString = "DELETE FROM Courses WHERE (CourseId = '" + course.CourseId + "')";
                    var objCtxDb = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    objCtxDb.ExecuteStoreCommand(queryString);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                            String.Format(" UCRS.Courses DELETE COURSE ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public int CourseCount()
        {
            int courseCount = db.Courses.Count();
            return courseCount;
        }
    }
}