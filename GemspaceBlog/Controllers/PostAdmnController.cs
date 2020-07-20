using GemspaceBlog.Models;
using GemspaceBlog.ViewModel;
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
    public class PostAdmnController : Controller
    {
        public DbModels dbModels = new DbModels();
        // GET: PostAdmn
        public ActionResult Index()
        {
            return View(dbModels.Posts.ToList());
        }

        // GET: PostAdmn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
            if (post == null) 
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: PostAdmn/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostAdmn/Create
        [HttpPost]
        public ActionResult Create(AdminViewModel adminViewModel)
        {
            if (ModelState.IsValid) {
                Post post = new Post();
                if (adminViewModel.Category == 1)
                {
                    post.Category = "Basketball";
                }
                else if (adminViewModel.Category == 2)
                {
                    post.Category = "Nature";
                }
                else if (adminViewModel.Category == 3)
                {
                    post.Category = "Food";
                }
                else if(adminViewModel.Category == 4)
                {
                    post.Category = "Coding";
                }
                else
                {
                    return View();
                }

                post.Id = adminViewModel.Id;
                post.Title = adminViewModel.Title;
                post.ShortDescription = adminViewModel.ShortDescription;
                post.LongDescription = adminViewModel.LongDescription;
                post.ReadTime = adminViewModel.ReadTime;

                if(PhotoValidation(adminViewModel.Image1File))
                {
                    post.Img1Path = SavePhoto(adminViewModel.Image1File);
                }

                if (PhotoValidation(adminViewModel.Image2File))
                {
                    post.Img2Path = SavePhoto(adminViewModel.Image2File);
                }

                post.CreatedAt = DateTime.Now;

                //Saving in DB
                dbModels.Posts.Add(post);
                dbModels.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index", "PostAdmn");
            }
                return View();
        }

        // GET: PostAdmn/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
            TempData["img1Path"] = post.Img1Path;
            TempData["img2Path"] = post.Img2Path;
            TempData["datetime"] = post.CreatedAt;

            AdminViewModel adminViewModel = new AdminViewModel();

            adminViewModel.Id = post.Id;
            adminViewModel.Title = post.Title;
            adminViewModel.ShortDescription = post.ShortDescription;
            adminViewModel.LongDescription = post.LongDescription;
            adminViewModel.ReadTime = post.ReadTime;
            adminViewModel.Img1Path = post.Img1Path;
            adminViewModel.Img2Path = post.Img2Path;
            adminViewModel.CreatedAt = post.CreatedAt;
            
            if (post.Category == "Basketball")
            {
                adminViewModel.Category = 1;
            }
            else if (post.Category == "Nature")
            {
                adminViewModel.Category = 2;
            }
            else if (post.Category == "Food")
            {
                adminViewModel.Category = 3;
            }
            else if (post.Category == "Coding")
            {
                adminViewModel.Category = 4;
            }

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(adminViewModel);
        }

        // POST: PostAdmn/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AdminViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post();
                if (adminViewModel.Image1File == null && adminViewModel.Image2File == null)
                {
                    post.Img1Path = TempData["img1Path"].ToString();
                    post.Img2Path = TempData["img2Path"].ToString();
                    post.CreatedAt = (DateTime)TempData["datetime"];
                    post.Id = adminViewModel.Id;
                    post.ShortDescription = adminViewModel.ShortDescription;
                    post.Title = adminViewModel.Title;
                    post.LongDescription = adminViewModel.LongDescription;
                    if (adminViewModel.Category == 1)
                    {
                        post.Category = "Basketball";
                    }
                    else if (adminViewModel.Category == 2)
                    {
                        post.Category = "Nature";
                    }
                    else if (adminViewModel.Category == 3)
                    {
                        post.Category = "Food";
                    }
                    else if (adminViewModel.Category == 4)
                    {
                        post.Category = "Coding";
                    }
                    else
                    {
                        return View();
                    }
                    post.ReadTime = adminViewModel.ReadTime;

                    dbModels.Entry(post).State = EntityState.Modified;
                    if (dbModels.SaveChanges() > 0)
                    {
                        TempData["msg"] = "Data Updated";
                        return RedirectToAction("Index");

                    }
                    else
                    {

                        TempData["msg"] = "Not Updated";
                        return RedirectToAction("Index");
                    }

                }
                else if (adminViewModel.Image1File != null && adminViewModel.Image2File == null)
                {
                    string oldPath = Request.MapPath(TempData["img1Path"].ToString());
                    post.Img1Path = SavePhoto(adminViewModel.Image1File);
                    post.Img2Path = TempData["img2Path"].ToString();
                    post.CreatedAt = (DateTime)TempData["datetime"];
                    post.Id = adminViewModel.Id;
                    post.ShortDescription = adminViewModel.ShortDescription;
                    post.Title = adminViewModel.Title;
                    post.LongDescription = adminViewModel.LongDescription;
                    if (adminViewModel.Category == 1)
                    {
                        post.Category = "Basketball";
                    }
                    else if (adminViewModel.Category == 2)
                    {
                        post.Category = "Nature";
                    }
                    else if (adminViewModel.Category == 3)
                    {
                        post.Category = "Food";
                    }
                    else if (adminViewModel.Category == 4)
                    {
                        post.Category = "Coding";
                    }
                    else
                    {
                        return View();
                    }
                    post.ReadTime = adminViewModel.ReadTime;
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
                else if (adminViewModel.Image1File == null && adminViewModel.Image2File != null)
                {
                    post.Img1Path = TempData["img1Path"].ToString();
                    post.Img2Path = SavePhoto(adminViewModel.Image2File);
                    post.CreatedAt = (DateTime)TempData["datetime"];
                    post.Id = adminViewModel.Id;
                    post.ShortDescription = adminViewModel.ShortDescription;
                    post.Title = adminViewModel.Title;
                    post.LongDescription = adminViewModel.LongDescription;
                    if (adminViewModel.Category == 1)
                    {
                        post.Category = "Basketball";
                    }
                    else if (adminViewModel.Category == 2)
                    {
                        post.Category = "Nature";
                    }
                    else if (adminViewModel.Category == 3)
                    {
                        post.Category = "Food";
                    }
                    else if (adminViewModel.Category == 4)
                    {
                        post.Category = "Coding";
                    }
                    else
                    {
                        return View();
                    }
                    post.ReadTime = adminViewModel.ReadTime;
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
                    post.Img1Path = SavePhoto(adminViewModel.Image1File);
                    post.Img2Path = SavePhoto(adminViewModel.Image2File);
                    post.CreatedAt = (DateTime)TempData["datetime"];
                    post.Id = adminViewModel.Id;
                    post.ShortDescription = adminViewModel.ShortDescription;
                    post.Title = adminViewModel.Title;
                    post.LongDescription = adminViewModel.LongDescription;
                    if (adminViewModel.Category == 1)
                    {
                        post.Category = "Basketball";
                    }
                    else if (adminViewModel.Category == 2)
                    {
                        post.Category = "Nature";
                    }
                    else if (adminViewModel.Category == 3)
                    {
                        post.Category = "Food";
                    }
                    else if (adminViewModel.Category == 4)
                    {
                        post.Category = "Coding";
                    }
                    else
                    {
                        return View();
                    }
                    post.ReadTime = adminViewModel.ReadTime;
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
            }
            return View();
        }

        // GET: PostAdmn/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: PostAdmn/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
                string postTitl = post.Title;
                string oldPath1 =  Request.MapPath(post.Img1Path);
                string oldPath2 =  Request.MapPath(post.Img2Path);

                dbModels.Posts.Remove(post);
                if (dbModels.SaveChanges() > 0) {
                    TempData["msg"] = "The post "+ postTitl + " got deleted";
                    if (System.IO.File.Exists(oldPath1))
                    {
                        System.IO.File.Delete(oldPath1);
                    }
                    if (System.IO.File.Exists(oldPath2))
                    {
                        System.IO.File.Delete(oldPath2);
                    }
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public bool PhotoValidation(HttpPostedFileBase file)
        {

            var extension = Path.GetExtension(file.FileName);
            if (extension == ".jpeg" || extension == ".png" || extension == ".jpg")
            {
                if (file.ContentLength < (2 * 1024 *1024))
                {
                    return true;
                }
                return false;
            }
            return false;
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
