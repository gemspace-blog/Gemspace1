using GemspaceBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace GemspaceBlog.Controllers
{
    public class HomeController : Controller
    {
        public DbModels dbModels = new DbModels();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Basketball()
        {
            return View();
        }

        public ActionResult Food()
        {
            return View();
        }

        public ActionResult Nature()
        {
            return View();
        }

        public ActionResult Coding()
        {
            return View();
        }

        public ActionResult Article(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();

            if(post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult AboutMe()
        {            
            return View();
        }
    }
}