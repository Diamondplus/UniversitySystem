using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public interface IStudentsRepository
    {
        IQueryable<Student> Students { get; }

        bool IsStudentExist(string email);

        bool IsStudentPasswordOk(string email, string password);

        bool AddStudent(string email, string password);

        bool DeleteStudent(string email);

        int StudentCount();
    }
}