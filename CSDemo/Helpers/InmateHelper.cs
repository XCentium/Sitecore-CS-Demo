using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using KeefePOC.Models;
using KeefePOC.Repositories;
using KeefePOC.Services;
using System;

namespace CSDemo.Helpers
{
    public class InmateHelper
    {
        public static string GetSelectedInmateId()
        {
            if (ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
                return "123456";
            }

            return GetSelectedInmate()?.InmateNumber;
        }

        public static Inmate GetSelectedInmate()
        {
            var cookie = Get("KEEF_INMATE");

            if (string.IsNullOrEmpty(cookie?.Value))
            {
                if (ConfigurationManager.AppSettings["DebugMode"] == "1")
                {
                    var inmateNo = GetSelectedInmateId();
                    return new Inmate
                    {
                        InmateNumber = GetSelectedInmateId(),
                        CurrentQuarterTotalOrderWeight = CurrentQuarterTotalOrderWeight(inmateNo),
                        CurrentQuarterTotalOrderPrice = CurrentQuarterTotalOrderPrice(inmateNo)
                    };
                }

                return null;
            }

            var inmate = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Inmate>(cookie.Value);

            if (inmate != null)
            {
                //populate quarterly totals from service
                inmate.CurrentQuarterTotalOrderWeight = CurrentQuarterTotalOrderWeight(inmate.InmateNumber);
                inmate.CurrentQuarterTotalOrderPrice = CurrentQuarterTotalOrderPrice(inmate.InmateNumber);
            }

            return inmate;
        }

        public static void SaveSelectedInmate(Inmate inmate)
        {

            if (inmate != null)
            {
                var cookie = Get("KEEF_INMATE");
                if (cookie != null)
                {
                    cookie.Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(inmate);
                    cookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Current.Response.SetCookie(cookie); // updates existing cookie, cookies.add.. can cause multiple cookies
                }
                else
                {
                    HttpCookie newCookie = new HttpCookie("KEEF_INMATE")
                    {
                        Expires = DateTime.Now.AddDays(365),
                        Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(inmate)
                    };
                    HttpContext.Current.Response.SetCookie(newCookie);
                }
            }
            else
            {
                var cookie = Get("KEEF_INMATE");
                if (cookie != null)
                {
                    // expire the cookie
                    cookie.Expires = DateTime.Now.AddDays(-30);
                    HttpContext.Current.Response.SetCookie(cookie); // updates existing cookie, cookies.add.. can cause multiple cookies
                }
            }
        }

        public static List<string> GetProductRestrictions()
        {
            var inmateId = GetSelectedInmateId();

            if (string.IsNullOrWhiteSpace(inmateId))
                return new List<string>();

            var svc = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
            return svc.GetProductRestrictionsForInmate(inmateId);
        }

        public static bool IsRestrictedMale()
        {
            var restrictions = GetProductRestrictions();

            return restrictions != null && restrictions.Any(r => r.Contains("male"));
        }

        public static bool IsRestrictedFemale()
        {
            var restrictions = GetProductRestrictions();

            return restrictions != null && restrictions.Any(r => r.Contains("female"));
        }

        public static bool IsRestrictedSugarFree()
        {
            var restrictions = GetProductRestrictions();

            return restrictions != null && restrictions.Any(r => r.Contains("sugarfree"));
        }

        public static bool IsRestrictedKosher()
        {
            var restrictions = GetProductRestrictions();

            return restrictions != null && restrictions.Any(r => r.Contains("kosher"));
        }

        public static bool IsRestrictedGlutenFree()
        {
            var restrictions = GetProductRestrictions();

            return restrictions != null && restrictions.Any(r => r.Contains("glutenfree"));
        }

        public static List<string> GetProductBlacklist()
        {
            var inmateId = GetSelectedInmateId();

            if (string.IsNullOrWhiteSpace(inmateId))
                return new List<string>();

            var svc = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
            return svc.GetBlacklistedItemsForInmate(inmateId);
        }

        public static HttpCookie Get(string cookieName)
        {
            return string.IsNullOrWhiteSpace(cookieName) ? null : HttpContext.Current.Request.Cookies[cookieName];
        }

        public static void Set(string cookieName, string cookieValue, int cookieExpirationInDays = 365)
        {

            var cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            cookie.Expires = DateTime.Now.AddDays(cookieExpirationInDays);

            //   cookie.Values[cookieName] = cookieValue;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        public static double CurrentQuarterTotalOrderWeight(string inmateId)
        {
            if (string.IsNullOrWhiteSpace(inmateId))
                return -1;

            var svc = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
            return svc.GetCurrentQuarterOrderTotalWeightForInmate(inmateId);
        }

        public static decimal CurrentQuarterTotalOrderPrice(string inmateId)
        {
            if (string.IsNullOrWhiteSpace(inmateId))
                return -1;

            var svc = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
            return svc.GetCurrentQuarterOrderTotalPriceForInmate(inmateId);
        }
    }
}