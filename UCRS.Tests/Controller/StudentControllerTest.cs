using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UCRS.Controllers;
using UCRS.Data;
using UCRS.Model;
using UCRS.Models;

namespace UCRS.Tests.Controller
{
    [TestClass]
    public class StudentControllerTest
    {
        [TestMethod]
        public void StudentController_ValidateStudentData_The_Email_And_Password_Are_OK_Test()
        {
            // Arrange
            Mock<IStudentsRepository> mockStudentRepository = new Mock<IStudentsRepository>();
            Mock<ICoursesRepository> mockCourseRepository = new Mock<ICoursesRepository>();
            Mock<IStudentsInCoursesRepository> mockStudentsInCoursesRepository = new Mock<IStudentsInCoursesRepository>();
            Mock<IFormsAuthenticat> formsAuthenticat = new Mock<IFormsAuthenticat>();

            var crypto = new SimpleCrypto.PBKDF2();
            crypto.HashIterations = 1;
            string passwordForAll = "1234567";
            string encrpPass = crypto.Compute(passwordForAll);
            string salt = crypto.Salt;

            Student[] students = new Student[]
                                    {
                                        new Student{ Email = "ivana.ivanova@ucrs.edu", Password=encrpPass, PasswordSalt=salt  },
                                        new Student{ Email = "petra.petrova@ucrs.edu", Password=encrpPass, PasswordSalt=salt  },
                                        new Student{ Email = "georgia.georgieva@ucrs.edu", Password=encrpPass, PasswordSalt=salt  }                 
                                    };
            mockStudentRepository.Setup(u => u.Students).Returns(students.AsQueryable());

            mockStudentRepository.Setup(u => u.IsStudentExist(It.IsAny<string>()))
                .Returns((string email) =>
                {
                    if (mockStudentRepository.Object.Students.FirstOrDefault(u => u.Email == email) != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                );

            mockStudentRepository.Setup(u => u.IsStudentPasswordOk(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns((string email, string password) =>
                                {
                                    var existingStudent = mockStudentRepository.Object.Students.Where(u => u.Email == email).FirstOrDefault();
                                    crypto = new SimpleCrypto.PBKDF2();
                                    crypto.HashIterations = 1; // normal = 5000 // Delay 
                                    if (existingStudent.Password == crypto.Compute(password, existingStudent.PasswordSalt))
                                    {
                                        // The username and password - OK
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                });

            // formsAuthenticat.Verify(x => x.Login("Aaron"));

            StudentController controller = new StudentController(mockStudentRepository.Object, mockCourseRepository.Object, mockStudentsInCoursesRepository.Object, formsAuthenticat.Object);

            // Act
            // Wanted User
            LoginStudentViewModel wantedStudent = new LoginStudentViewModel
            {
                Email = "georgia.georgieva@ucrs.edu",
                Password = "1234567"
            };
            PartialViewResult result = (PartialViewResult)controller.ValidateStudentData(wantedStudent);
            var values = result.ViewData.ModelState.Values;

            List<string> errors = new List<string>();
            foreach (var modelStateValue in values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    errors.Add(error.ErrorMessage);
                    // var exception = error.Exception;
                }
            }

            // Assert
            Assert.AreEqual(0, errors.Count);
        }


        [TestMethod]
        public void StudentController_ValidateStudentData_The_Email_Does_Not_Exist_Test()
        {
            // Arrange
            Mock<IStudentsRepository> mockStudentRepository = new Mock<IStudentsRepository>();
            Mock<ICoursesRepository> mockCourseRepository = new Mock<ICoursesRepository>();
            Mock<IStudentsInCoursesRepository> mockStudentsInCoursesRepository = new Mock<IStudentsInCoursesRepository>();
            Mock<IFormsAuthenticat> formsAuthenticat = new Mock<IFormsAuthenticat>();

            var crypto = new SimpleCrypto.PBKDF2();
            crypto.HashIterations = 1;
            string passwordForAll = "1234567";
            string encrpPass = crypto.Compute(passwordForAll);
            string salt = crypto.Salt;

            Student[] students = new Student[]
                                    {
                                        new Student{ Email = "ivana.ivanova@ucrs.edu", Password=encrpPass, PasswordSalt=salt  },
                                        new Student{ Email = "petra.petrova@ucrs.edu", Password=encrpPass, PasswordSalt=salt  },
                                        new Student{ Email = "georgia.georgieva@ucrs.edu", Password=encrpPass, PasswordSalt=salt  }                 
                                    };
            mockStudentRepository.Setup(u => u.Students).Returns(students.AsQueryable());

            mockStudentRepository.Setup(u => u.IsStudentExist(It.IsAny<string>()))
                .Returns((string email) =>
                {
                    if (mockStudentRepository.Object.Students.FirstOrDefault(u => u.Email == email) != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                );

            mockStudentRepository.Setup(u => u.IsStudentPasswordOk(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns((string email, string password) =>
                                {
                                    var existingStudent = mockStudentRepository.Object.Students.Where(u => u.Email == email).FirstOrDefault();
                                    crypto = new SimpleCrypto.PBKDF2();
                                    crypto.HashIterations = 1; // normal = 5000 // Delay 
                                    if (existingStudent.Password == crypto.Compute(password, existingStudent.PasswordSalt))
                                    {
                                        // The username and password - OK
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                });

            // formsAuthenticat.Verify(x => x.Login("Aaron"));

            StudentController controller = new StudentController(mockStudentRepository.Object, mockCourseRepository.Object, mockStudentsInCoursesRepository.Object, formsAuthenticat.Object);

            // Act
            // Wanted User
            LoginStudentViewModel wantedStudent = new LoginStudentViewModel
            {
                Email = "unexist.email@ucrs.edu",
                Password = "1234567"
            };
            PartialViewResult result = (PartialViewResult)controller.ValidateStudentData(wantedStudent);
            var values = result.ViewData.ModelState.Values;

            List<string> errors = new List<string>();
            foreach (var modelStateValue in values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    errors.Add(error.ErrorMessage);
                    // var exception = error.Exception;
                }
            }

            // Assert
            Assert.AreEqual("The email does not exist", errors[0]);
        }

        [TestMethod]
        public void StudentController_ValidateStudentData_The_Email_Exist_But_Password_Is_Wrong_Test()
        {
            // Arrange
            Mock<IStudentsRepository> mockStudentRepository = new Mock<IStudentsRepository>();
            Mock<ICoursesRepository> mockCourseRepository = new Mock<ICoursesRepository>();
            Mock<IStudentsInCoursesRepository> mockStudentsInCoursesRepository = new Mock<IStudentsInCoursesRepository>();
            Mock<IFormsAuthenticat> formsAuthenticat = new Mock<IFormsAuthenticat>();

            var crypto = new SimpleCrypto.PBKDF2();
            crypto.HashIterations = 1;
            string passwordForAll = "1234567";
            string encrpPass = crypto.Compute(passwordForAll);
            string salt = crypto.Salt;

            Student[] students = new Student[]
                                    {
                                        new Student{ Email = "ivana.ivanova@ucrs.edu", Password=encrpPass, PasswordSalt=salt  },
                                        new Student{ Email = "petra.petrova@ucrs.edu", Password=encrpPass, PasswordSalt=salt  },
                                        new Student{ Email = "georgia.georgieva@ucrs.edu", Password=encrpPass, PasswordSalt=salt  }                 
                                    };
            mockStudentRepository.Setup(u => u.Students).Returns(students.AsQueryable());

            mockStudentRepository.Setup(u => u.IsStudentExist(It.IsAny<string>()))
                .Returns((string email) =>
                {
                    if (mockStudentRepository.Object.Students.FirstOrDefault(u => u.Email == email) != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                );

            mockStudentRepository.Setup(u => u.IsStudentPasswordOk(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns((string email, string password) =>
                                {
                                    var existingStudent = mockStudentRepository.Object.Students.Where(u => u.Email == email).FirstOrDefault();
                                    crypto = new SimpleCrypto.PBKDF2();
                                    crypto.HashIterations = 1; // normal = 5000 // Delay 
                                    if (existingStudent.Password == crypto.Compute(password, existingStudent.PasswordSalt))
                                    {
                                        // The username and password - OK
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                });

            // formsAuthenticat.Verify(x => x.Login("Aaron"));

            StudentController controller = new StudentController(mockStudentRepository.Object, mockCourseRepository.Object, mockStudentsInCoursesRepository.Object, formsAuthenticat.Object);

            // Act
            // Wanted User
            LoginStudentViewModel wantedStudent = new LoginStudentViewModel
            {
                Email = "ivana.ivanova@ucrs.edu",
                Password = "wrongPass"
            };
            PartialViewResult result = (PartialViewResult)controller.ValidateStudentData(wantedStudent);
            var values = result.ViewData.ModelState.Values;

            List<string> errors = new List<string>();
            foreach (var modelStateValue in values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    errors.Add(error.ErrorMessage);
                    // var exception = error.Exception;
                }
            }

            // Assert
            Assert.AreEqual("Incorrect email/password", errors[0]);
        }



    }


}
