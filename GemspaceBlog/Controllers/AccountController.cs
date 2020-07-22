using GemspaceBlog.Models;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            Login loginregister = new Login();
            loginregister.UserName = "Xhemi";
            loginregister.Password = encoder.Encode("Xhema2297");
            loginEntity.Logins.Add(loginregister);
            loginEntity.SaveChanges();

            Login loginDb = loginEntity.Logins.Where(x => x.UserName == login.UserName).FirstOrDefault();

            return RedirectToAction("Index", "PostAdmn");
        }

        public ActionResult SaveDb()
        {
            ScryptEncoder encoder = new ScryptEncoder();
            Login loginregister = new Login();
            loginregister.UserName = "Xhemi";
            loginregister.Password = encoder.Encode("Xhema2297");
            loginEntity.Logins.Add(loginregister);
            loginEntity.SaveChanges();
            return RedirectToAction("Index", "PostAdmn");

        }
    }
}