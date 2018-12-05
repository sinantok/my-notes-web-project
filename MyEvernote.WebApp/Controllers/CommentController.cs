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
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();

        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //ENote note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id.Value);
            List<EComment> comments = commentManager.ListQueryable().Where(x => x.Note.Id == id.Value).ToList();

            if (comments == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments", comments);
        }

        [Authenticationn]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EComment comment = commentManager.Find(x => x.Id == id.Value);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;

            if (commentManager.Update(comment) > 0)
            {
                return Json(new { sonuc = true }, JsonRequestBehavior.AllowGet); //if operation true
            }

            return Json(new { sonuc = false }, JsonRequestBehavior.AllowGet);

        }

        [Authenticationn]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MessageResult<EComment> res = commentManager.RemoveCommentById(id.Value);
            if (res.Errors.Count == 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Authenticationn]
        [HttpPost]
        public ActionResult Create(string text, int? noteId)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                if (noteId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ENote note = noteManager.Find(x => x.Id == noteId);

                if (note == null)
                {
                    return new HttpNotFoundResult();
                }
                if (text == "") return new HttpNotFoundResult();
                EComment comment = new EComment();
                comment.Text = text;
                comment.Note = note;
                comment.Owner = SessionManager.User;

                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { ress = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { ress = false }, JsonRequestBehavior.AllowGet);
        }
    }
}