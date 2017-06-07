using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using CSDemo.Configuration;
using CSDemo.Helpers;
using CSDemo.Models.Checkout.Cart;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Diagnostics;

namespace CSDemo.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class XCommerceService : IXCommerceService
    {
        public List<Movie> GetShowTimes(string zipcode, int hours)
        {
            var movies = new List<Movie>();

            try
            {
                Assert.IsNotNullOrEmpty(zipcode, "zipcode is null or empty");

                //get nearest cinema based on user's zipcode
                var nearestCinemaZipcode = GetNearestCinemaZipcode(zipcode);

                Assert.IsNotNullOrEmpty(nearestCinemaZipcode, "nearestCinemaZipcode is null or empty");

                //check hours and move to default 3 if <= 0
                hours = hours <= 0 ? 3 : hours;

                var moviesSearchResults = GetMoviesByZipcode(nearestCinemaZipcode);

                return moviesSearchResults
                    .Select(m => new Movie
                {
                    Title = m.Document.Fields[Movie.Fields.MovieName]?.ToString(),
                    CinemaName = m.Document.Fields[Movie.Fields.CinemaName]?.ToString(),
                    ShowTime = m.Document.Fields[Movie.Fields.ShowTime]?.ToString(),
                    Price = m.Document.Fields[Movie.Fields.Price] == null ? 0 : double.Parse(m.Document.Fields[Movie.Fields.Price].ToString()),
                    ShowDate = m.Document.Fields[Movie.Fields.ShowDate]?.ToString(),
                    CinemaId = m.Document.Fields[Movie.Fields.CinemaId]?.ToString(),
                    CinemaZipcode = m.Document.Fields[Movie.Fields.CinemaZipCode]?.ToString(),
                    Id = new Guid(m.Document.Fields[Movie.Fields.VariantId].ToString()).ToString()
                })
                    .Where(m => DateTime.Parse(DateTime.Today.ToShortDateString() + " " + m.ShowTime.ToString()) >= DateTime.Now 
                        && DateTime.Parse(DateTime.Today.ToShortDateString() + " " + m.ShowTime.ToString()) <= DateTime.Now.AddHours(hours))
                    .OrderBy(m => m.Title).ThenBy(m => m.CinemaName).ThenBy(m => m.ShowDate).ThenBy(m => m.ShowTime).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"CSDemo.Services.XCommerceService.GetShowTimes, Error = {ex.Message}", ex);
            }

            return movies;
        }

        private string GetNearestCinemaZipcode(string customerZipcode)
        {
            string cinemaZipcode = null;

            var zipcodeSvcApiKey = ConfigurationHelper.GetZipcodeServiceApiKey();
            var zipcodeSvcApiCallFormat = ConfigurationHelper.GetZipcodeServiceCallFormat();
            var zipcodes = GetCinemaLocationZipcodes();

            if (zipcodes.Contains(customerZipcode))
            {
                //customer is in same zip code as a cinema
                return customerZipcode;
            }

            //find nearest
            var zipcodeList = $"{customerZipcode},{GetCinemaLocationZipcodes()}";
            var zipcodeSvcApiCall = zipcodeSvcApiCallFormat.Replace("{APIKEY}", zipcodeSvcApiKey)
                .Replace("{ZIPCODES}", zipcodeList);

            //do call
            var client = new WebClient();
            var jsonResponse = client.DownloadString(zipcodeSvcApiCall);

            if (!string.IsNullOrWhiteSpace(jsonResponse))
            {
                //var json = JsonConvert.DeserializeObject(jsonResponse);
                var responses = JsonConvert.DeserializeObject<List<ZipcodeServiceResponse>>(jsonResponse);

                if (responses != null && responses.Any())
                {
                    var closest = responses.Where(c => c.ZipCode1.Contains(customerZipcode) || c.ZipCode2.Contains(customerZipcode)).OrderBy(c => c.Distance).First();
                    cinemaZipcode = closest.ZipCode1 == customerZipcode ? closest.ZipCode2 : closest.ZipCode1;
                }
            }

            return cinemaZipcode;
        }

        private static string GetCinemaLocationZipcodes()
        {
            var zipcodes = string.Empty;

            const string location = "/sitecore/content/XCinemaDemo/Components/Cinemas";

            var cinemasItem = Context.Database.GetItem(location);

            if (cinemasItem == null) return zipcodes;

            var cinemaList = GlassHelper.Cast<CinemaList>(cinemasItem);
            if (cinemaList != null) zipcodes = cinemaList.GetZipcodes();

            return zipcodes;
        }

        public MovieOrder BuyMovie(MovieOrder order)
        {
            try
            {
                int orderQty;

                if (string.IsNullOrWhiteSpace(order?.MovieVariantId) 
                    || string.IsNullOrWhiteSpace(order?.NoOfTickets)
                    || string.IsNullOrWhiteSpace(order?.CustomerEmailAddress)
                    || !int.TryParse(order.NoOfTickets, out orderQty))
                {
                    throw new ArgumentException("Argument errror. Please check required fields.");
                }

                const string shopName = "XCinemaDemo";
                var cartHelper = new CartHelper(shopName);

                order = cartHelper.QuickBuyMovie(order);
            }
            catch (Exception ex)
            {
                if (order != null)
                {
                    order.IsOrderSuccessful = false;
                    order.Message = $"CSDemo.Services.XCommerceService.BuyMovie, Error = {ex.Message}";
                }

                Log.Error($"CSDemo.Services.XCommerceService.BuyMovie, Error = {ex.Message}", ex);
            }

            return order;
        }

        //questions on buy movie
        //1] will it enable anonymous buying? if so, the email address will need to be sent at least to send the tickets to
        //2] address? name?
        //3] username in system if not anonymous
        //4] payment details
        //5] phase 1 - movieId, qty, email address (payment will be faked)
        //6] how to get payent processed? user has to have payment info registered for "One click buy"
        //7] add items are digital items (no physical)

        private static IEnumerable<SearchHit<SearchResultItem>> GetMoviesByZipcode(string zipcode)
        {
            var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndexMovies());
            try
            {
                using (var context = index.CreateSearchContext())
                {

                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);

                    return
                        queryable.Where(
                            x =>
                                x["moviezipcode"].Contains(zipcode) &&
                                x["_latestversion"] == "1" &&
                                x.TemplateName == "MovieVariant").GetResults().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);
            }
            return null;
        }
    }
}
