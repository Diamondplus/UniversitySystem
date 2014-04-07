using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public class StudentsRepository : IStudentsRepository
    {
        private UniversityContext db = new UniversityContext();

        public IQueryable<Student> Students
        {
            get { return db.Students.AsQueryable(); }
        }

        public bool IsStudentExist(string email)
        {
            Student dbEntry = db.Students.FirstOrDefault(s => s.Email == email);
            if (dbEntry == null)
            {
                return false; 
            }
            else
            {
                return true;
            }
        }

        public bool IsStudentPasswordOk(string email, string password)
        {
            var existingStudent = db.Students.FirstOrDefault(s => s.Email == email);
            var crypto = new SimpleCrypto.PBKDF2();
            crypto.HashIterations = 1; // normal = 5000 // Delay 
            if (existingStudent != null && existingStudent.Password == crypto.Compute(password, existingStudent.PasswordSalt))
            {
                // The email and password - OK
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddStudent(string email, string password)
        {
            Student dbEntry = db.Students.FirstOrDefault(s => s.Email == email);
            if (dbEntry == null)
            {
                try
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    crypto.HashIterations = 1; // normal = 5000
                    var encrpPass = crypto.Compute(password);
                    var newStudent = db.Students.Create();

                    newStudent.Email = email.Trim();
                    newStudent.Password = encrpPass;
                    newStudent.PasswordSalt = crypto.Salt;

                    db.Students.Add(newStudent);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                        String.Format(" UCRS.Students NEW USER ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool DeleteStudent(string email)
        {
            Student dbEntry = db.Students.FirstOrDefault(s => s.Email == email);
            if (dbEntry != null)
            {
                try
                {
                    string queryString = "DELETE FROM Students WHERE (Email = '" + email.Trim() + "')";
                    var objCtxDb = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    objCtxDb.ExecuteStoreCommand(queryString);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(
                            String.Format(" UCRS.Students DELETE ONE STUDENT ERROR:\\n\\n{0}", ex.Message)));
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public int StudentCount()
        {
            int studentCount = db.Students.Count();
            return studentCount;
        }
    }
}