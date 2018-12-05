using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.ViewModels
{
    public class OkViewModel : NotifyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "Transaction Successful";
        }
    }
}
