using GemspaceBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GemspaceBlog.Controllers
{
    public class AdminController : Controller
    {
        public DbModels dbModels = new DbModels();
        // GET: Admin
        public ActionResult Index()
        {
                return View(dbModels.Posts.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
            return View(post);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(Post post)
        {
            try
            {
                //Saving the pgotos at the file
                post.Img1Path = SavePhoto(post.Image1File);
                post.Img2Path = SavePhoto(post.Image2File);

                //Saving in DB
                dbModels.Posts.Add(post);
                dbModels.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
            TempData["img1Path"] = post.Img1Path;
            TempData["img2Path"] = post.Img2Path;
            if (post == null) {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Post post)
        {

            if (post.Image1File == null && post.Image2File == null)
            {
                post.Img1Path = TempData["img1Path"].ToString();
                post.Img2Path = TempData["img2Path"].ToString();
                dbModels.Entry(post).State = EntityState.Modified;
                if (dbModels.SaveChanges() > 0)
                {
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");

                }

            }
            else if (post.Image1File != null && post.Image2File == null)
            {
                post.Img1Path = SavePhoto(post.Image1File);
                string oldPath = Request.MapPath(TempData["img1Path"].ToString());
                post.Img2Path = TempData["img2Path"].ToString();
                dbModels.Entry(post).State = EntityState.Modified;
                if (dbModels.SaveChanges() > 0)
                {
                    if (System.IO.File.Exists(oldPath)) {
                        System.IO.File.Delete(oldPath);
                    }
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");

                }
            }
            else if (post.Image1File == null && post.Image2File != null)
            {
                post.Img1Path = TempData["img1Path"].ToString();
                post.Img2Path = SavePhoto(post.Image2File);
                string oldPath = Request.MapPath(TempData["img2Path"].ToString());
                dbModels.Entry(post).State = EntityState.Modified;
                if (dbModels.SaveChanges() > 0)
                {
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");

                }
            }
            else 
            {
                post.Img1Path = SavePhoto(post.Image1File); 
                post.Img2Path = SavePhoto(post.Image2File);
                string oldPath1 = Request.MapPath(TempData["img1Path"].ToString());
                string oldPath2 = Request.MapPath(TempData["img2Path"].ToString());
                dbModels.Entry(post).State = EntityState.Modified;
                if (dbModels.SaveChanges() > 0) 
                {
                    if (System.IO.File.Exists(oldPath1))
                    {
                        System.IO.File.Delete(oldPath1);
                    }
                    if (System.IO.File.Exists(oldPath2))
                    {
                        System.IO.File.Delete(oldPath2);
                    }
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");

                }
            }

            return View();
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
            return View(post); 
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
                dbModels.Posts.Remove(post);
                dbModels.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public string SavePhoto(HttpPostedFileBase file) 
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extention = Path.GetExtension(file.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
            String dbPath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            file.SaveAs(fileName);
            return dbPath;
        
        }
    }
}
