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
    public class CommentManager : ManagerBase<EComment>
    {
        public MessageResult<EComment> RemoveCommentById(int id)
        {
         
            MessageResult<EComment> res = new MessageResult<EComment>();
            res.Result = Find(x => x.Id == id);

            if (res.Result != null)
            {
                
                if (Delete(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCodes.CommentCouldNotRemove, "Comment could not be deleted.");
                }
                return res;
            }
            else
            {
                res.AddError(ErrorMessageCodes.CommentCouldNotFind, "Comment could not find.");
                return res;
            }

        }
    }
}
