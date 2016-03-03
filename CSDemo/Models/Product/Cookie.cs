using System;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Extensions;

namespace CSDemo.Models.Product
{
    public class Cookie
    {
        public static void SaveFeaturedProductCookie(string value)
        {
            if (!value.IsEmptyOrNull())
            {
                var cookie = Get("FeaturedProducts");
                if (cookie != null)
                {
                    var ids = cookie.Value.Split(',').ToList();
                    if (ids.All(t => t != value))
                    {
                        ids.Add(value);
                    }
                    cookie.Value = string.Join(",", ids);
                    cookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Current.Response.SetCookie(cookie); // updates existing cookie, cookies.add.. can cause multiple cookies
                }
                else
                {
                    HttpCookie newCookie = new HttpCookie("FeaturedProducts")
                    {
                        Expires = DateTime.Now.AddDays(365),
                        Value = value
                    };
                    HttpContext.Current.Response.SetCookie(newCookie);
                }
            }
        }

        public static HttpCookie Get(string cookieName)
        {
            return string.IsNullOrWhiteSpace(cookieName) ? null : HttpContext.Current.Request.Cookies[cookieName];
        }
    }
}