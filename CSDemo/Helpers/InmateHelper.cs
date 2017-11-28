using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeefePOC.Models;
using KeefePOC.Repositories;
using KeefePOC.Services;

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
            return HttpContext.Current?.Session["SELECTED_INMATE"] as Inmate;
        }

        public static void SaveSelectedInmate(Inmate inmate)
        {
            HttpContext.Current.Session["SELECTED_INMATE"] = inmate;
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
    }
}