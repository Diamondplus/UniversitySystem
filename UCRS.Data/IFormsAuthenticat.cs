using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UCRS.Data
{
    public interface IFormsAuthenticat
    {
        void Login(string email);
        void Logout();
    }
}