using GemspaceBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using PagedList.Mvc;
using PagedList;

namespace GemspaceBlog.Controllers
{
    public class HomeController : Controller
    {
        public DbModels dbModels = new DbModels();
        public ActionResult Index(int? i)
        {
            return View(dbModels.Posts.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(i ?? 1, 5));
        }

        public ActionResult Basketball(int? i)
        {
            return View(dbModels.Posts.Where(x=>x.Category == "Basketball").OrderByDescending( z => z.CreatedAt).ToList().ToPagedList(i ?? 1, 5));
        }

        public ActionResult Food(int? i)
        {
            return View(dbModels.Posts.Where(x =>x.Category == "Food").OrderByDescending(z => z.CreatedAt).ToList().ToPagedList(i ?? 1 , 5 ));
        }

        public ActionResult Nature(int? i)
        {
            return View(dbModels.Posts.Where(x => x.Category == "Nature").OrderByDescending(z => z.CreatedAt).ToList().ToPagedList(i ?? 1, 5));
        }

        public ActionResult Coding(int? i)
        {
            return View(dbModels.Posts.Where(x => x.Category == "Coding").OrderByDescending(z => z.CreatedAt).ToList().ToPagedList(i ?? 1, 5));
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