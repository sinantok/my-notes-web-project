using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ResultModel;
using MyEvernote.Entities.ViewModels;
using MyEvernote.WebApp.Filters;
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
    public class UserController : Controller
    {
        // GET: User
        private UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            //if(Session["user"]==null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            return View(userManager.ListQueryable().OrderByDescending(x => x.ModifiedOn));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MessageResult<EUser> res = new MessageResult<EUser>();
            res = userManager.GetUserById(id.Value);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "User could not find",
                    RedirectingUrl = "/User/Index",
                    RedirectingTimeOut = 2000
                };
                errorView.Items = new List<ErrorMessageObj>();
                errorView.Items = res.Errors;
                return View("Error", errorView);
            }
            
            return View(res.Result);
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
        public ActionResult Create(EUser model)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("CreateOn");
            if (ModelState.IsValid)
            {
                MessageResult<EUser> res = userManager.Insert(model);

                if (res.Errors.Count > 0)
                {
                    foreach (var item in res.Errors)
                    {
                        ModelState.AddModelError("", item.Message);
                    }

                    return View(model);
                }
                return RedirectToAction("Index", "User");
            }
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MessageResult<EUser> res = new MessageResult<EUser>();
            res = userManager.GetUserById(id.Value);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "User could not find",
                    RedirectingUrl = "/User/Index",
                    RedirectingTimeOut = 2000
                };
                errorView.Items = new List<ErrorMessageObj>();
                errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            return View(res.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EUser model)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("CreateOn");
            if (ModelState.IsValid)
            {
                MessageResult<EUser> res = userManager.Update(model);
                if (res.Errors.Count > 0)
                {
                    foreach (var item in res.Errors)
                    {
                        ModelState.AddModelError("", item.Message);
                    }
                    return View(model);
                }
                return RedirectToAction("Index", "User");
            }
            return View(model);
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

            MessageResult<EUser> res = userManager.RemoveUserById(id.Value);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "User could not be deleted",
                    RedirectingUrl = "/User/Index"
                };
                errorView.Items = new List<ErrorMessageObj>();
                errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            return RedirectToAction("Index", "User");
        }
    }
}