using GemspaceBlog.Models;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GemspaceBlog.Controllers
{
    public class AccountController : Controller
    {
        GemspaceEntitiesLogin loginEntity = new GemspaceEntitiesLogin();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            ScryptEncoder encoder = new ScryptEncoder();

            var loginDb = loginEntity.Logins.Where(x => x.UserName == login.UserName).SingleOrDefault();

            bool isValid = encoder.Compare(login.Password, loginDb.Password);
            if (isValid)
            {
                FormsAuthentication.SetAuthCookie(login.UserName, false);
                string ReturnUrl = "/PostAdmn/Index";
                return Redirect(ReturnUrl);

            }

            return View();

        }
    }
}