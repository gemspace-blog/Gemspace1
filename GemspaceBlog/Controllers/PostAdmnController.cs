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
using PagedList.Mvc;
using PagedList;

namespace GemspaceBlog.Controllers
{
    [Authorize]
    public class PostAdmnController : Controller
    {
        public DbModels dbModels = new DbModels();
        // GET: PostAdmn
        // This is a method called Index within the PostAdmn Controller 
        // the url to access this is /PostAdmn/Index

        public ActionResult Index(int? i)
        {
            return View(dbModels.Posts.ToList().ToPagedList(i ?? 1, 5));
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
            if (ModelState.IsValid)
            {
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
                else if (adminViewModel.Category == 4)
                {
                    post.Category = "Coding";
                }
                else
                {
                    TempData["msg"] = "Validation Failed";
                    return View();
                }

                post.Id = adminViewModel.Id;
                post.Title = adminViewModel.Title;
                post.ShortDescription = adminViewModel.ShortDescription;
                post.LongDescription = adminViewModel.LongDescription;
                post.ReadTime = adminViewModel.ReadTime;

                if (PhotoValidation(adminViewModel.Image1File))
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
                TempData["msg"] = "Post Created";
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

            if (post == null)
            {
                return HttpNotFound();
            }

            TempData["img1Path"] = post.Img1Path;
            TempData["img2Path"] = post.Img2Path;
            TempData["datetime"] = post.CreatedAt;

            EditViewModel editViewModel = new EditViewModel();

            editViewModel.Id = post.Id;
            editViewModel.Title = post.Title;
            editViewModel.ShortDescription = post.ShortDescription;
            editViewModel.LongDescription = post.LongDescription;
            editViewModel.ReadTime = post.ReadTime;
            editViewModel.Img1Path = post.Img1Path;
            editViewModel.Img2Path = post.Img2Path;
            editViewModel.CreatedAt = post.CreatedAt;

            if (post.Category == "Basketball")
            {
                editViewModel.Category = 1;
            }
            else if (post.Category == "Nature")
            {
                editViewModel.Category = 2;
            }
            else if (post.Category == "Food")
            {
                editViewModel.Category = 3;
            }
            else if (post.Category == "Coding")
            {
                editViewModel.Category = 4;
            }

            ViewBag.editModel = editViewModel;
            return View(editViewModel);
        }

        // POST: PostAdmn/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EditViewModel editViewModel)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post();
                if (editViewModel.Image1File == null && editViewModel.Image2File == null)
                {
                    post.Img1Path = (string)TempData["img1Path"];
                    post.Img2Path = (string)TempData["img2Path"];
                    post.CreatedAt = (DateTime)TempData["datetime"];
                    post.Id = editViewModel.Id;
                    post.ShortDescription = editViewModel.ShortDescription;
                    post.Title = editViewModel.Title;
                    post.LongDescription = editViewModel.LongDescription;
                    post.ReadTime = editViewModel.ReadTime;
                    if (editViewModel.Category == 1)
                    {
                        post.Category = "Basketball";
                    }
                    else if (editViewModel.Category == 2)
                    {
                        post.Category = "Nature";
                    }
                    else if (editViewModel.Category == 3)
                    {
                        post.Category = "Food";
                    }
                    else if (editViewModel.Category == 4)
                    {
                        post.Category = "Coding";
                    }
                    else
                    {
                        return View(ToVM(dbModels.Posts.Where(x => x.Id == id).FirstOrDefault()));
                    }

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
                else if (editViewModel.Image1File != null && editViewModel.Image2File == null)
                {
                    string fileName1 = Path.GetFileName(editViewModel.Image1File.FileName);
                    string _fileName1 = DateTime.Now.ToString("yymmssfff") + fileName1;

                    string extension = Path.GetExtension(editViewModel.Image1File.FileName);

                    string path = Path.Combine(Server.MapPath("~/Image/"), _fileName1);

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (editViewModel.Image1File.ContentLength <= 2097152)
                        {
                            post.Img1Path = "~/Image/" + _fileName1;
                            post.Img2Path = (string)TempData["img2Path"];
                            post.CreatedAt = (DateTime)TempData["datetime"];
                            post.Id = editViewModel.Id;
                            post.ShortDescription = editViewModel.ShortDescription;
                            post.Title = editViewModel.Title;
                            post.LongDescription = editViewModel.LongDescription;
                            post.ReadTime = editViewModel.ReadTime;
                            if (editViewModel.Category == 1)
                            {
                                post.Category = "Basketball";
                            }
                            else if (editViewModel.Category == 2)
                            {
                                post.Category = "Nature";
                            }
                            else if (editViewModel.Category == 3)
                            {
                                post.Category = "Food";
                            }
                            else if (editViewModel.Category == 4)
                            {
                                post.Category = "Coding";
                            }
                            else
                            {
                                return View(ToVM(dbModels.Posts.Where(x => x.Id == id).FirstOrDefault()));
                            }
                            dbModels.Entry(post).State = EntityState.Modified;
                            string oldImgPath1 = (string)TempData["img1Path"];

                            if (dbModels.SaveChanges() > 0)
                            {
                                editViewModel.Image1File.SaveAs(path);
                                if (System.IO.File.Exists(oldImgPath1))
                                {
                                    System.IO.File.Delete(oldImgPath1);
                                }
                                TempData["msg"] = "Data Updated";
                                return RedirectToAction("Index");
                            }

                        }
                        else
                        {
                            ViewBag.msg = "File Size must be Equal or less than 2 MB";
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Invalid File Type";
                    }
                }
                else if (editViewModel.Image1File == null && editViewModel.Image2File != null)
                {
                    string fileName2 = Path.GetFileName(editViewModel.Image2File.FileName);
                    string _fileName2 = DateTime.Now.ToString("yymmssfff") + fileName2;

                    string extension2 = Path.GetExtension(editViewModel.Image2File.FileName);

                    string path2 = Path.Combine(Server.MapPath("~/Image/"), _fileName2);

                    if (extension2.ToLower() == ".jpg" || extension2.ToLower() == ".jpeg" || extension2.ToLower() == ".png")
                    {
                        if (editViewModel.Image2File.ContentLength <= 2097152)
                        {

                            post.Img2Path = "~/Image/" + _fileName2;
                            post.Img1Path = (string)TempData["img2Path"];
                            post.CreatedAt = (DateTime)TempData["datetime"];
                            post.Id = editViewModel.Id;
                            post.ShortDescription = editViewModel.ShortDescription;
                            post.Title = editViewModel.Title;
                            post.LongDescription = editViewModel.LongDescription;
                            post.ReadTime = editViewModel.ReadTime;
                            if (editViewModel.Category == 1)
                            {
                                post.Category = "Basketball";
                            }
                            else if (editViewModel.Category == 2)
                            {
                                post.Category = "Nature";
                            }
                            else if (editViewModel.Category == 3)
                            {
                                post.Category = "Food";
                            }
                            else if (editViewModel.Category == 4)
                            {
                                post.Category = "Coding";
                            }
                            else
                            {
                                return View(ToVM(dbModels.Posts.Where(x => x.Id == id).FirstOrDefault()));
                            }
                            dbModels.Entry(post).State = EntityState.Modified;
                            string oldImgPath2 = (string)TempData["img2Path"];

                            if (dbModels.SaveChanges() > 0)
                            {
                                editViewModel.Image2File.SaveAs(path2);
                                if (System.IO.File.Exists(oldImgPath2))
                                {
                                    System.IO.File.Delete(oldImgPath2);
                                }
                                TempData["msg"] = "Data Updated";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.msg = "File Size must be Equal or less than 2 MB";
                        }

                    }
                    else
                    {
                        ViewBag.msg = "Invalid File Type";
                    }
                }
                else
                {
                    string fileName12 = Path.GetFileName(editViewModel.Image1File.FileName);
                    string _fileName12 = DateTime.Now.ToString("yymmssfff") + fileName12;

                    string extension12 = Path.GetExtension(editViewModel.Image1File.FileName);

                    string path12 = Path.Combine(Server.MapPath("~/Image/"), _fileName12);

                    string fileName22 = Path.GetFileName(editViewModel.Image2File.FileName);
                    string _fileName22 = DateTime.Now.ToString("yymmssfff") + fileName22;

                    string extension22 = Path.GetExtension(editViewModel.Image2File.FileName);

                    string path22 = Path.Combine(Server.MapPath("~/Image/"), _fileName22);


                    if ((extension12.ToLower() == ".jpg" || extension12.ToLower() == ".jpeg" || extension12.ToLower() == ".png") && (extension22.ToLower() == ".jpg" || extension22.ToLower() == ".jpeg" || extension22.ToLower() == ".png"))
                    {
                        if (editViewModel.Image1File.ContentLength <= 2097152 && editViewModel.Image2File.ContentLength <= 2097152)
                        {
                            post.Img2Path = "~/Image/" + _fileName22;
                            post.Img1Path = "~/Image/" + _fileName12;
                            post.CreatedAt = (DateTime)TempData["datetime"];
                            post.Id = editViewModel.Id;
                            post.ShortDescription = editViewModel.ShortDescription;
                            post.Title = editViewModel.Title;
                            post.LongDescription = editViewModel.LongDescription;
                            post.ReadTime = editViewModel.ReadTime;
                            if (editViewModel.Category == 1)
                            {
                                post.Category = "Basketball";
                            }
                            else if (editViewModel.Category == 2)
                            {
                                post.Category = "Nature";
                            }
                            else if (editViewModel.Category == 3)
                            {
                                post.Category = "Food";
                            }
                            else if (editViewModel.Category == 4)
                            {
                                post.Category = "Coding";
                            }
                            else
                            {
                                return View(ToVM(dbModels.Posts.Where(x => x.Id == id).FirstOrDefault()));
                            }
                            dbModels.Entry(post).State = EntityState.Modified;
                            string oldImgPath22 = (string)TempData["img2Path"];
                            string oldImgPath12 = (string)TempData["img1Path"];

                            if (dbModels.SaveChanges() > 0)
                            {
                                editViewModel.Image2File.SaveAs(path22);
                                editViewModel.Image1File.SaveAs(path12);
                                if (System.IO.File.Exists(oldImgPath12))
                                {
                                    System.IO.File.Delete(oldImgPath12);
                                }

                                if (System.IO.File.Exists(oldImgPath22))
                                {
                                    System.IO.File.Delete(oldImgPath22);
                                }
                                TempData["msg"] = "Data Updated";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.msg = "File Size must be Equal or less than 2 MB";
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Invalid File Type";
                    }

                }
            }
            return View(ToVM(dbModels.Posts.Where(x => x.Id == id).FirstOrDefault()));
        }

        // GET: PostAdmn/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: PostAdmn/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Post post = dbModels.Posts.Where(x => x.Id == id).FirstOrDefault();
                string postTitl = post.Title;
                string oldPath1 = Request.MapPath(post.Img1Path);
                string oldPath2 = Request.MapPath(post.Img2Path);

                dbModels.Posts.Remove(post);
                if (dbModels.SaveChanges() > 0)
                {
                    TempData["msg"] = "The post " + postTitl + " got deleted";
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
                if (file.ContentLength < (2 * 1024 * 1024))
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

        public EditViewModel ToVM(Post post)
        {
            EditViewModel editVM = new EditViewModel
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                LongDescription = post.LongDescription,
                ReadTime = post.ReadTime,
                CreatedAt = post.CreatedAt,
                Img1Path = post.Img1Path,
                Img2Path = post.Img2Path
            };

            if (post.Category == "Basketball")
            {
                editVM.Category = 1;
            }
            else if (post.Category == "Nature")
            {
                editVM.Category = 2;
            }
            else if (post.Category == "Food")
            {
                editVM.Category = 3;
            }
            else if (post.Category == "Coding")
            {
                editVM.Category = 4;
            }

            return editVM;
        }
    }
}
