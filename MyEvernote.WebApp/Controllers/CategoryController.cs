using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ResultModel;
using MyEvernote.Entities.ViewModels;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    [Excep]
    [Authenticationn]//1
    [AuthenticationAdmin]//2
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();

        public ActionResult Index()
        {
            //if(Session["user"]==null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            return View(categoryManager.List().OrderByDescending(x => x.ModifiedOn));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ECategory cat = categoryManager.Find(x => x.Id == id.Value);
              
            if (cat == null)
            {
                return new HttpNotFoundResult();
            }

            return View(cat);
        }

        public ActionResult Create()
        {
            //if (Session["user"] == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ECategory category)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("CreateOn");
            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                CacheManager.Remove("category-cache");
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ECategory cat = categoryManager.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ECategory mdel)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("CreateOn");

            if (ModelState.IsValid)
            {
                ECategory cat = categoryManager.Find(x => x.Id == mdel.Id);
                if (cat != null)
                {
                    cat.Title = mdel.Title;
                    cat.Description = mdel.Description;
                    if(categoryManager.Update(cat) == 0)
                    {
                        ModelState.AddModelError("", "DataBaseError!");
                        return View(mdel);
                    }

                    CacheManager.Remove("category-cache");
                    //HttpContext.Cache.Remove("category-cache");

                    return RedirectToAction("Index", "Category");
                }
                ModelState.AddModelError("dbNot", "DataBaseError!");
                return View(mdel);
            }
            return View(mdel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //ECategory cat = categoryManager.Find(x => x.Id == id.Value);
            //if (cat == null)
            //{
            //    return HttpNotFound();
            //}

            MessageResult<ECategory> res = categoryManager.RemoveCategoryById(id.Value);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "Category could not remove",
                    RedirectingUrl = "/Category/Index"
                };
                errorView.Items = new List<ErrorMessageObj>();
                errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            CacheManager.Remove("category-cache");

            return RedirectToAction("Index", "Category");
        }

    }
}