using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.DataAccesLayer.EntityFramework;
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
    public class NoteManager : ManagerBase<ENote>
    {
        public MessageResult<ENote> RemoveNoteById(int id)
        {
            LikeManager likeManager = new LikeManager();
            CommentManager commentManager = new CommentManager();

            MessageResult<ENote> res = new MessageResult<ENote>();
            res.Result = Find(x => x.Id == id);

            if (res.Result != null)
            {
                foreach (EComment item in res.Result.Comments.ToList())
                {
                    commentManager.Delete(item);
                }

                foreach (ELiked like in res.Result.Likes.ToList())
                {
                    likeManager.Delete(like);
                }

                if (Delete(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCodes.CategoryCouldNotRemove, "Note could not be deleted.");
                }
                return res;
            }
            else
            {
                res.AddError(ErrorMessageCodes.NoteCouldNotFind, "Note could not find.");
                return res;
            }

        }

    }
}
