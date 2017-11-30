using System.Collections.Generic;
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
			return GetSelectedInmate()?.InmateNumber;
		}

		public static Inmate GetSelectedInmate()
		{

			var cookie = Get("KEEF_INMATE");

			if (cookie == null || string.IsNullOrEmpty(cookie.Value))
			{
				return null;
			}

			return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Inmate>(cookie.Value);

			//return HttpContext.Current?.Session["SELECTED_INMATE"] as Inmate;
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

			//HttpContext.Current.Session["SELECTED_INMATE"] = inmate;
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
	}
}