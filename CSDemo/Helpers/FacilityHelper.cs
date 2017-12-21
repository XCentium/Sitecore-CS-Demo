﻿using System;
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

				blacklist = facility.ProgramCategories.ToList();
			}
			catch (Exception e)
			{
				Log.Error($"FacilityHelper.GetProgramProductCategoryBlacklist(), Error = {e.Message}", e);
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
			var cookie = HttpContext.Current.Request.Cookies["SELECTED_FACILITY"];

			if (cookie == null || string.IsNullOrEmpty(cookie.Value))
			{
				if (ConfigurationManager.AppSettings["DebugMode"] == "1")
				{
					return new Facility
					{
						Id = GetSelectedFacilityId()
					};
				}

				return null;
			}

			return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Facility>(cookie.Value);
		}

		public static void SaveSelectedFacility(Facility modelSelectedFacility)
		{

			if (modelSelectedFacility != null)
			{
				var cookie = HttpContext.Current.Request.Cookies["SELECTED_FACILITY"];
				if (cookie != null)
				{
					cookie.Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(modelSelectedFacility);
					cookie.Expires = DateTime.Now.AddDays(365);
					HttpContext.Current.Response.SetCookie(cookie); // updates existing cookie, cookies.add.. can cause multiple cookies
				}
				else
				{
					var newCookie = new HttpCookie("SELECTED_FACILITY")
					{
						Expires = DateTime.Now.AddDays(365),
						Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(modelSelectedFacility)
					};
					HttpContext.Current.Response.SetCookie(newCookie);
				}
			}
			else
			{
				var cookie = HttpContext.Current.Request.Cookies["SELECTED_FACILITY"];
				if (cookie != null)
				{
					// expire the cookie
					cookie.Expires = DateTime.Now.AddDays(-30);
					HttpContext.Current.Response.SetCookie(cookie); // updates existing cookie, cookies.add.. can cause multiple cookies
				}
			}
		}

		public static Models.Checkout.Cart.Address GetFacilityAddress()
		{
			var facilityId = GetSelectedFacilityId();

			if (string.IsNullOrWhiteSpace(facilityId))
				return null;

			var facilityItem = Sitecore.Context.Database.GetItem(facilityId);
			var facility = GlassHelper.Cast<FacilityModel>(facilityItem);

			if (facility != null)
				return new CSDemo.Models.Checkout.Cart.Address() { Address1 = facility.AddressLine1, Address2 = facility.AddressLine2, Country = facility.Country, City = facility.City, ZipPostalCode = facility.PostalCode, State = facility.State };

			return null;
		}

		public static Item GetFacilityByExternalId(string associatedFacilityId)
		{
			const string facilityLocation = "/sitecore/content/Global Configuration/Facilities";

			var folder = Sitecore.Context.Database.GetItem(facilityLocation);

			var facilityItem = folder.Children.FirstOrDefault(c => c["ExternalID"] == associatedFacilityId);


			//var facility = GlassHelper.Cast<FacilityModel>(facilityItem);

			return facilityItem;
		}
	}
}