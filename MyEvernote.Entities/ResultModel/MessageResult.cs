using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.ResultModel
{
    public class MessageResult<T> where T : class
    {
        public List<ErrorMessageObj> Errors { get; set; }

        public T Result { get; set; }

        public MessageResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        //error message insertion method(prevents code overload)
        public void AddError(ErrorMessageCodes code, string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code, Message = message });
        }

    }
}
