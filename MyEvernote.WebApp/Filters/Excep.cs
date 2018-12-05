using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Filters
{
    public class Excep : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["LastError"] = filterContext.Exception; //onlyOneRequest
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("/Home/HasError");
        }
    }
}