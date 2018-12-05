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
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private UserManager userManager = new UserManager();

        // GET: Home
        public ActionResult Index()
        {
            //throw new Exception("any an error");
            //Test test = new Test();
            //test.UpdateTest();
            //test.CommnetTest();

            //notesById ile alakalı.Category controllerından geliyor bu veri.
            ///if (TempData["notesBy"] != null)
            ///{
            ///    return View(TempData["notesBy"] as List<ENote>);
            ///}

            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.CreateOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ENote> notes = noteManager.ListQueryable().Where(x => x.IsDraft == false && x.CategoryId == id.Value).
                OrderByDescending(x => x.CreateOn).ToList();
            if (notes == null)
            {
                return HttpNotFound();
            }

            return View("Index", notes);
        }

        public ActionResult MostLiked()
        {
            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginModel)
        {   
            if(ModelState.IsValid)
            {
                MessageResult<EUser> res = userManager.LoginUser(loginModel);
                if (res.Errors.Count > 0)
                {
                    foreach (var item in res.Errors)
                    {
                        ModelState.AddModelError("", item.Message);
                    }

                    if (res.Errors.Find(x => x.Code == ErrorMessageCodes.UserIsnotActive) != null)
                    {
                        //ViewBag.SendCode = true;
                    }
                    return View(loginModel);
                }

                //Session["user"] = res.Result;
                SessionManager.Set<EUser>("user", res.Result);
                return RedirectToAction("Index","Home");
            }
            return View(loginModel);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel kayitModel)
        {
            if(ModelState.IsValid)
            {
                MessageResult<EUser> res = userManager.RegisterUser(kayitModel);
                if (res.Errors.Count > 0)
                {
                    foreach (var item in res.Errors)
                    {
                        ModelState.AddModelError("", item.Message);
                    }
                    return View(kayitModel);
                }

                OkViewModel okView = new OkViewModel()
                {
                    Title = "Register Successful",
                    RedirectingUrl = "/Home/Login",
                    RedirectingTimeOut = 5000,
                    Items = new List<string>(){ "Please activate your account by clicking on the activation link sent to your email address." },
                };
                return View("Ok", okView);
            }

            return View(kayitModel);
        }

        public ActionResult UserActive(Guid id)
        {
            //account acvivation
            MessageResult<EUser> res = userManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "Invalid Operation",
                };
                errorView.Items = new List<ErrorMessageObj>(); errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            OkViewModel okView = new OkViewModel()
            {
                Title = "Account Activated",
                Items = new List<string>() { "Your account successfully activated." },
                RedirectingTimeOut = 3000,
                RedirectingUrl = "/Home/Login",
            };
            TempData["aktiveUser"] = (res.Result.Username).ToString();

            return View("Ok", okView);
        }

        public ActionResult Logout()
        {
            if (SessionManager.User != null)
            {
                SessionManager.Claer("user");
                return RedirectToAction("Index", "Home");
            }
            return HttpNotFound();
        }

        [Authenticationn]
        public ActionResult ShowProfile()
        {
            MessageResult<EUser> res = userManager.GetUserById(SessionManager.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "An Error Occurred!",
                };
                errorView.Items = new List<ErrorMessageObj>(); errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            return View(res.Result);
        }

        [Authenticationn]
        public ActionResult EditProfile()
        {
            MessageResult<EUser> res = userManager.GetUserById(SessionManager.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "An Error Occurred!",
                };
                errorView.Items = new List<ErrorMessageObj>(); errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            return View(res.Result);
        }

        [Authenticationn]
        [HttpPost]
        public ActionResult EditProfile(EUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUser");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                 ProfileImage.ContentType == "image/jpg" ||
                 ProfileImage.ContentType == "image/png"))
                {
                    string fileName = $"user_{ model.Id}.{ ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/UserImages/{fileName}"));
                    model.ProfileImageFileName = fileName;
                }

                MessageResult<EUser> res = userManager.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorView = new ErrorViewModel()
                    {
                        Title = "Failed to update profile",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    errorView.Items = new List<ErrorMessageObj>(); errorView.Items = res.Errors;
                    return View("Error", errorView);
                }

                SessionManager.Set<EUser>("user", res.Result);
                return RedirectToAction("ShowProfile", "Home");
            }
            return View(model);
        }

        [Authenticationn]
        public ActionResult DeleteProfile()
        {
            MessageResult<EUser> res = userManager.RemoveUserById(SessionManager.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "Failed to remove profile",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                errorView.Items = new List<ErrorMessageObj>(); errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }

        ////////////////
        public ActionResult LoginTest()
        {
            return View();
        }

        //public PartialViewResult CategoryAll()
        //{
        //    CategoryManager cm = new CategoryManager();
        //    List<ECategory> List = cm.GetECategories();
        //    return PartialView("_PartialCategories",List);
        //}
    }
}