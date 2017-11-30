using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CSDemo.Models;
using CSDemo.Models.Product;
using KeefePOC.Models;
using Sitecore.Diagnostics;
using Sitecore.Data.Items;

namespace CSDemo.Helpers
{
    public class FacilityHelper
    {
        public static List<Category> GetProgramProductCategoryBlacklist()
        {
            var blacklist = new List<Category>();

            try
            {
                var facilityId = GetSelectedFacilityId();

                if (string.IsNullOrWhiteSpace(facilityId))
                    return blacklist;

                var facilityItem = Sitecore.Context.Database.GetItem(facilityId);
                var facility = GlassHelper.Cast<FacilityModel>(facilityItem);

                if (facility == null) return blacklist;

                blacklist = facility.BlackListedProductCategories.ToList();
            }
            catch (Exception e)
            {
                Log.Error($"FacilityHelper.GetProgramProductCateogoryBlacklist(), Error = {e.Message}", e);
            }

            return blacklist;
        }

        private static string GetSelectedFacilityId()
        {
            if (ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
                return "{7CBB94CB-F321-4428-B05E-21540E8B4A58}";
            }

            return GetSelectedFacility()?.Id;
        }

        private static Facility GetSelectedFacility()
        {
            var facility = HttpContext.Current.Session["SELECTED_FACILITY"] as Facility;

            if (facility == null && ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
               return new Facility
               {
                   Id = GetSelectedFacilityId()
               };
            }

            return facility;
        }

        public static void SaveSelectedFacility(Facility modelSelectedFacility)
        {
            HttpContext.Current.Session["SELECTED_FACILITY"] = modelSelectedFacility;
        }

		public static CSDemo.Models.Checkout.Cart.Address GetFacilityAddress()
		{			
			var facilityId = GetSelectedFacilityId();

			if (string.IsNullOrWhiteSpace(facilityId))
				return null;

			var facilityItem = Sitecore.Context.Database.GetItem(facilityId);
			var facility = GlassHelper.Cast<FacilityModel>(facilityItem);

			if (facility != null) return new CSDemo.Models.Checkout.Cart.Address() {  Address1 = facility.AddressLine1, Address2 = facility.AddressLine2, Country = facility.Country, City = facility.City,  ZipPostalCode = facility.PostalCode, State = facility.State };

			return null;
		}

		internal static Item GetFacilityByExternalId(string associatedFacilityId)
		{
			string facilityLocation = "/sitecore/content/Global Configuration/Facilities";		

			var folder = Sitecore.Context.Database.GetItem(facilityLocation);

			var facilityItem = folder.Children.FirstOrDefault(c => c["ExternalID"] == associatedFacilityId);

			
			//var facility = GlassHelper.Cast<FacilityModel>(facilityItem);

			return facilityItem;
		}
	}
}