using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ResultModel;
using MyEvernote.Entities.ViewModels;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    [Excep]
    public class NoteController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private LikeManager likeManager = new LikeManager();

        // GET: Note
        [Authenticationn]
        public ActionResult Index()
        {
            //bring the related table with related tables. The purpose name in the model is written
            var notes = noteManager.ListQueryable().
                Include("Category").Include("Owner").
                Where(x => x.Owner.Id == SessionManager.User.Id).
                OrderByDescending(x => x.CreateOn);

            return View(notes.ToList());
        }

        [Authenticationn]
        public ActionResult MyLikedNotes()
        {
            var liked = likeManager.ListQueryable().
                Include("LikedUser").Include("Note").
                Where(x => x.LikedUser.Id == SessionManager.User.Id).
                Select(x => x.Note).Include("Category").Include("Owner").
                OrderByDescending(x => x.CreateOn);

            return View("Index", liked.ToList());
        }

        [Authenticationn]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ENote note = noteManager.Find(x => x.Id == id.Value);

            if(note == null)
            {
                return new HttpNotFoundResult();
            }

            return View(note);
        }

        [Authenticationn]
        public ActionResult Create()
        {
            // ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", model.CategoryId);
            ViewBag.CategoryId = new SelectList(CacheManager.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        [Authenticationn]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ENote model)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("CreateOn");

            if (ModelState.IsValid)
            {
                model.Owner = SessionManager.User;
                noteManager.Insert(model);
                return RedirectToAction("Index", "Note");
            }

            ViewBag.CategoryId = new SelectList(CacheManager.GetCategoriesFromCache(), "Id", "Title", model.CategoryId);
            return View(model);
        }

        [Authenticationn]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ENote note = noteManager.Find(x => x.Id == id.Value);

            if (note == null)
            {
                return new HttpNotFoundResult();
            }

            ViewBag.CategoryId = new SelectList(CacheManager.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [Authenticationn]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ENote model)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("CreateOn");

            if (ModelState.IsValid)
            {
                ENote note = noteManager.Find(x => x.Id == model.Id);
                if (note != null)
                {
                    note.CategoryId = model.CategoryId;
                    note.Title = model.Title;
                    note.Text = model.Text;
                    note.IsDraft = model.IsDraft;
                    if (noteManager.Update(note) == 0)
                    {
                        ModelState.AddModelError("", "DataBaseError!");
                        return View(model);
                    }
                    return RedirectToAction("Index", "Note");
                }
                ModelState.AddModelError("dbNot", "DataBaseError!");
                return View(model);
            }

            ViewBag.CategoryId = new SelectList(CacheManager.GetCategoriesFromCache(), "Id", "Title", model.CategoryId);
            return View(model);
        }

        [Authenticationn]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MessageResult<ENote> res = noteManager.RemoveNoteById(id.Value);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorView = new ErrorViewModel()
                {
                    Title = "Note could not remove",
                    RedirectingUrl = "/Note/Index"
                };
                errorView.Items = new List<ErrorMessageObj>();
                errorView.Items = res.Errors;
                return View("Error", errorView);
            }

            return RedirectToAction("Index", "Note");
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (SessionManager.User != null)
            {
                if (ids != null)
                {
                    List<int> likedNoteIds = likeManager.
                        ListQueryable().Where(x => ids.Contains(x.Note.Id) && x.Note.IsDraft == false && x.LikedUser.Id == SessionManager.User.Id).
                        Select(x => x.Note.Id).ToList(); //contains--sql/in

                    return Json(new { ress = likedNoteIds });
                }
                return Json(new { ress = new List<int>() });
            }

            return Json(new { ress = new List<int>() });

        }

        [HttpPost]
        public ActionResult SetLikes(int noteId, bool liked)
        {
            int res = 0;

            if (SessionManager.User == null)
            {
                return Json(new { hasErrorr = true, errorMessagee = "You must entry in system for to like.", ress = 0 });
            }
            ELiked like = likeManager.Find(x => x.Note.Id == noteId && x.LikedUser.Id == SessionManager.User.Id);

            ENote note = noteManager.Find(x => x.Id == noteId);

            if (like != null && liked == false)
            {
                res = likeManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likeManager.Insert(new ELiked()
                {
                    LikedUser = SessionManager.User,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                
                res = noteManager.Update(note);

                return Json(new { hasErrorr = false, errorMessagee = string.Empty, ress = note.LikeCount });
            }

            return Json(new { hasErrorr = true, errorMessagee = "Failed.", ress = note.LikeCount });
        }

        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ENote note = noteManager.Find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialNoteText", note);
        }
    }
}