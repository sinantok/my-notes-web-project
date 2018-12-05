using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.Models
{
    public class CacheManager
    {
        public static List<ECategory> GetCategoriesFromCache()
        {
            var res = HttpContext.Current.Cache["category-cache"];
            if(res == null)
            {
                CategoryManager categoryManager = new CategoryManager();
                res = categoryManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
                HttpContext.Current.Cache["category-cache"] = res;
            }

            return res as List<ECategory>;
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}