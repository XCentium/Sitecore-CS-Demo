using System.Collections.Generic;
using System.Configuration;
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
            if (ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
                return "123456";
            }

            return GetSelectedInmate()?.InmateNumber;
        }

        public static Inmate GetSelectedInmate()
        {
            var inmate = HttpContext.Current?.Session["SELECTED_INMATE"] as Inmate;

            if (inmate == null && ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
                var inmateNo = GetSelectedInmateId();
                return new Inmate
                {
                    InmateNumber = GetSelectedInmateId(),
                    CurrentQuarterTotalOrderWeight = CurrentQuarterTotalOrderWeight(inmateNo),
                    CurrentQuarterTotalOrderPrice = CurrentQuarterTotalOrderPrice(inmateNo)
            };
            }

            if (inmate != null) { 
                //populate quarterly totals from service
                inmate.CurrentQuarterTotalOrderWeight = CurrentQuarterTotalOrderWeight(inmate.InmateNumber);
                inmate.CurrentQuarterTotalOrderPrice = CurrentQuarterTotalOrderPrice(inmate.InmateNumber);
            }

            return inmate;
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