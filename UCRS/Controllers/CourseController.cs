using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UCRS.Model;
using UCRS.Data;

namespace UCRS.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICoursesRepository coursesRepository;
        private readonly IStudentsInCoursesRepository studentsInCoursesRepository;
        private readonly IFormsAuthenticat formsAuthenticat;

        public CourseController()
        {;
            this.coursesRepository = new CoursesRepository();
            this.studentsInCoursesRepository = new StudentsInCoursesRepository();
            this.formsAuthenticat = new FormsAuthenticat();
        }

        public CourseController( ICoursesRepository coursesRepo,
                                 IStudentsInCoursesRepository studentsInCoursesRepo, 
                                 IFormsAuthenticat authentication)
        {
            this.coursesRepository = coursesRepo;
            this.studentsInCoursesRepository = studentsInCoursesRepo;
            this.formsAuthenticat = authentication;
        }

        // GET: /Course/
        public ActionResult Index()
        {
            List<Course> allCourses = coursesRepository.Courses.ToList();
            return View(allCourses);
        }

        // GET: /Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = coursesRepository.FindCourse(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: /Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CourseId,Name")] Course course)
        {
            if (ModelState.IsValid)
            {
                coursesRepository.AddCourse(course);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: /Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = coursesRepository.FindCourse(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CourseId,Name")] Course course)
        {
            if (ModelState.IsValid)
            {
                coursesRepository.UpdateCourse(course);
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: /Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = coursesRepository.FindCourse(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = coursesRepository.FindCourse(id);
            coursesRepository.DeleteCourse(course);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                coursesRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
