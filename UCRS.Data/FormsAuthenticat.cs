using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace UCRS.Data
{
    public class FormsAuthenticat : IFormsAuthenticat
    {
        public void Login(string email)
        {
            FormsAuthentication.SetAuthCookie(email, false);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}