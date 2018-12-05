using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Filters
{
    public class Authenticationn : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(SessionManager.User == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}