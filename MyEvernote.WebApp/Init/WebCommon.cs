using MyEvernote.Common;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["user"] != null)
            {
                EUser user = HttpContext.Current.Session["user"] as EUser;
                return user.Username;
            }
            return "System";
        }
    }
}