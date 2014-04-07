using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using UCRS.Data;
using UCRS.Model;
using UCRS.Models;

namespace UCRS.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentsRepository studentsRepository;
        private readonly ICoursesRepository coursesRepository;
        private readonly IStudentsInCoursesRepository studentsInCoursesRepository;
        private readonly IFormsAuthenticat formsAuthenticat;

        public StudentController()
        {
            this.studentsRepository = new StudentsRepository();
            this.coursesRepository = new CoursesRepository();
            this.studentsInCoursesRepository = new StudentsInCoursesRepository();
            this.formsAuthenticat = new FormsAuthenticat();
        }

        public StudentController(IStudentsRepository studentsRepo, ICoursesRepository coursesRepo,
                                    IStudentsInCoursesRepository studentsInCoursesRepo, IFormsAuthenticat authentication)
        {
            this.studentsRepository = studentsRepo;
            this.coursesRepository = coursesRepo;
            this.studentsInCoursesRepository = studentsInCoursesRepo;
            this.formsAuthenticat = authentication;
        }

        public ActionResult List()
        {
            List<Student> allStudents = studentsRepository.Students.ToList();
            List<int> coursesPerStudent = new List<int>();
            foreach (var student in allStudents)
            {
                coursesPerStudent.Add(studentsInCoursesRepository.NumberOfCoursesStudent(student.StudentId));
            }

            ViewBag.CoursesPerStudent = coursesPerStudent;
            return View(allStudents);
        }

        /// <summary>
        /// Index @ Home
        /// </summary>
        /// <returns> Redirect to Index@Home </returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Login through ValidateUserData@User
        /// </summary>
        /// <param name="student" type="LoginStudentViewModel student"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogIn(LoginStudentViewModel student)
        {
            return View(student);
        }

        /// <summary>
        /// Validate Email and crypted Passwords
        /// Create needed errors
        /// </summary>
        /// <param name="student" type="LoginStudentViewModel user"></param>
        /// <returns type="PartialView(IEnumerable<ModelError>)"></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ValidateStudentData(LoginStudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                if (studentsRepository.IsStudentExist(student.Email))
                {
                    if (studentsRepository.IsStudentPasswordOk(student.Email, student.Password))
                    {
                        // The email and password - OK
                        formsAuthenticat.Login(student.Email);
                        return PartialView(null);
                    }
                    else
                    {
                        // The email exist, but the password is wrong 
                        ModelState.AddModelError("", "Incorrect email/password");
                    }
                }
                else
                {
                    // The email does not exist 
                    ModelState.AddModelError("", "The email does not exist");
                }
            }
            else
            {
                // ModelState is wrong, but student exist? ( missing Password )
                if (student.Email != null)
                {

                    if (studentsRepository.IsStudentExist(student.Email.Trim()))
                    {
                        // The password is wrong 
                        ModelState.AddModelError("", "Incorrect email/password");
                    }
                    else
                    {
                        // The email does not exist 
                        ModelState.AddModelError("", "The email does not exist");
                    }
                }
                else
                {
                    // The email is null 
                    ModelState.AddModelError("", "Please, enter Email");
                }
            }
            IEnumerable<ModelError> modelStateErrors = ModelState.Values.SelectMany(v => v.Errors);
            return PartialView(modelStateErrors);
        }


        /// <summary>
        /// Sign In
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// Sign In - Validates Emails and Passwords: Creates needed errors
        /// Records Email, crypted Password, PasswordSalt 
        /// </summary>
        /// <param name="student" type="LoginStudentViewModel user"></param>
        /// <returns>If ok redirect to Login</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(LoginStudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                if (studentsRepository.IsStudentExist(student.Email.Trim()))
                {
                    ModelState.AddModelError("", "Unable to sign up - username already exists !");
                }
                else
                {
                    // Make new record in UCRS.Students: Email, Password(crypted), PasswordSalt, Id
                    studentsRepository.AddStudent(student.Email, student.Password);

                    return RedirectToAction("Login", "Student", new LoginStudentViewModel { Email = Encode(student.Email), Password = Encode(student.Password) });
                }
            }
            else
            {
                if (student.Email != null)
                {
                    // Model is wrong, but Email exist
                    if (studentsRepository.IsStudentExist(student.Email.Trim()))
                    {
                        ModelState.AddModelError("", "Unable to sign up - email already exists !");
                    }
                }
            }
            return View(student);
        }

        /// <summary>
        /// SignUp 
        /// </summary>
        /// <returns>
        /// Redirect to Logut()
        /// </returns>
        [HttpGet]
        public ActionResult SignOut()
        {
            string email = User.Identity.Name;
            formsAuthenticat.Logout();
            return RedirectToAction("Logout", new { encodedEmail = Encode(email) });
        }

        /// <summary>
        /// LogOut
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout(string encodedEmail)
        {
            ViewBag.Username = Decode(encodedEmail);
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult MyCourses()
        {
            string email = User.Identity.Name;
            if (email == null)
            {
                return View("You are not logged to see and manage courses");
            }
           
            Student student = studentsRepository.Students.FirstOrDefault(s => s.Email == email);
            List<Course> regCourses = coursesRepository.GetRegisteredCourses(student.StudentId);
            List<Course> unregCourses = coursesRepository.GetUnregisteredCourses(student.StudentId);
            MyCoursesViewModel regAndUnregCourses = new MyCoursesViewModel( student, regCourses, unregCourses);
            return View(regAndUnregCourses);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult _AddRegCourse(int studentId, int courseId)
        {   
            studentsInCoursesRepository.AddStudentInCourse(studentId, courseId);
            List<Course>regCourses = coursesRepository.GetRegisteredCourses(studentId);
            return Json(regCourses, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult _RemoveAllRegCourses(int studentId)
        {
            studentsInCoursesRepository.DeleteAllMyCourses(studentId);
            List<Course> allCourses = coursesRepository.Courses.ToList();
            return Json( allCourses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Encode Email and Password @Address
        /// </summary>
        /// <returns></returns>
        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        /// <summary>
        /// Decode Email and Password @Address
        /// </summary>
        /// <returns></returns>
        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }



    }
}