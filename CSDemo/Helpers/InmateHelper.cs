using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeefePOC.Repositories;
using KeefePOC.Services;

namespace CSDemo.Helpers
{
    public class InmateHelper
    {
        public static string GetSelectedInmateId()
        {
            return HttpContext.Current?.Session["SELECTED_INMATE"]?.ToString();
        }

        public static List<string> GetProductRestrictions()
        {
            //var inmateId = GetSelectedInmateId();
            var inmateId = "123451"; //TODO: remove hardcoded inmateId

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
    }
}