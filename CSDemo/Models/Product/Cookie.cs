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


        /// <summary>
        /// Create a new Cookie
        /// </summary>
        /// <param name="cookieName">Cookie Name</param>
        /// <param name="cookieExpirationInDays">Expiration Days</param>
        /// <param name="cookieValue">Cookie value as a string</param>
        public static void Set(string cookieName, string cookieValue, int cookieExpirationInDays = 365)
        {

            var cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            cookie.Expires = DateTime.Now.AddDays(cookieExpirationInDays);

            //   cookie.Values[cookieName] = cookieValue;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }



        internal static void Del(string cookieName)
        {
            var cartCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cartCookie != null)
            {
                // Render cookie invalid
                HttpContext.Current.Response.Cookies.Remove(cookieName);
                cartCookie.Expires = DateTime.Now.AddDays(-10);
                cartCookie.Values[cookieName] = null;
                cartCookie.Value = null;
                HttpContext.Current.Response.SetCookie(cartCookie);
            }
        }
    }
}