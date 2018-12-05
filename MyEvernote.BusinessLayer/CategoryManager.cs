using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<ECategory>
    {
        public MessageResult<ECategory> RemoveCategoryById(int id)
        {
            NoteManager noteManager = new NoteManager();
            LikeManager likeManager = new LikeManager();
            CommentManager commentManager = new CommentManager();

            MessageResult<ECategory> res = new MessageResult<ECategory>();
            res.Result = Find(x => x.Id == id);
            if (res.Result != null)
            {
                //delete
                foreach (ENote note in res.Result.Notes.ToList())
                {
                    //delete like
                    foreach (ELiked like in note.Likes.ToList())
                    {
                        likeManager.Delete(like);
                    }

                    //comment delete 
                    foreach (EComment comment in note.Comments.ToList())
                    {
                        commentManager.Delete(comment);
                    }

                    //note delete
                    noteManager.Delete(note);
                }

                if (Delete(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCodes.CategoryCouldNotRemove, "Category could not be deleted.");
                }
                return res;
            }
            else
            {
                res.AddError(ErrorMessageCodes.UserCouldNotFind, "User could not find.");
            }

            return res;
        }
    }
}
