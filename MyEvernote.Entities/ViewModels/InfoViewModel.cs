using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.ViewModels
{
    public class InfoViewModel : NotifyViewModelBase<string>
    {
        public InfoViewModel()
        {
            Title = "Information!";
        }
    }
}
