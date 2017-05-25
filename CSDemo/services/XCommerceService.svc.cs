using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using CSDemo.Configuration;
using CSDemo.Models.Checkout.Cart;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Diagnostics;

namespace CSDemo.Services
{
    [ServiceContract(Namespace = "XCommerceService")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class XCommerceService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Movie> GetMovies(string zipcode)
        {
            var movies = new List<Movie>();

            try
            {
                if (string.IsNullOrWhiteSpace(zipcode))
                {
                    throw new ArgumentException("zipcode is null or empty");
                }

                var moviesSearchResults = GetMoviesByZipcode(zipcode);
                return moviesSearchResults.Select(m => new Movie
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
                    .OrderBy(m => m.Title).ThenBy(m => m.CinemaName).ThenBy(m => m.ShowDate).ThenBy(m => m.ShowTime).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"CSDemo.Services.XCommerceService.GetMovies, Error = {ex.Message}", ex);
            }

            return movies;
        }

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
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

        public static IEnumerable<SearchHit<SearchResultItem>> GetMoviesByZipcode(string zipcode)
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

    [SitecoreType(AutoMap = true), DataContract]
    public class Movie
    {
        [SitecoreField(Fields.VariantId), DataMember]
        public string Id { get; set; }
        [SitecoreField(Fields.MovieName), DataMember]
        public string Title { get; set; }
        [SitecoreField(Fields.Price), DataMember]
        public double Price { get; set; }
        [SitecoreField(Fields.ShowDate), DataMember]
        public string ShowDate { get; set; }
        [SitecoreField(Fields.ShowTime), DataMember]
        public string ShowTime { get; set; }
        [SitecoreField(Fields.CinemaName), DataMember]
        public string CinemaName { get; set; }
        [SitecoreField(Fields.CinemaZipCode), DataMember]
        public string CinemaZipcode { get; set; }
        [SitecoreField(Fields.CinemaId), DataMember]
        public string CinemaId { get; set; }

        public struct Fields
        {
            public const string VariantId = "_group";
            public const string MovieName = "moviename";
            public const string Price = "listprice";
            public const string ShowDate = "showdate";
            public const string ShowTime = "showtime";
            public const string CinemaName = "movielocationname";
            public const string CinemaZipCode = "moviezipcode";
            public const string CinemaId = "locationid";
        }
    }

    [DataContract]
    public class MovieOrder
    {
        [DataMember]
        public string MovieVariantId { get; set; }
        [DataMember]
        public string NoOfTickets { get; set; }
        [DataMember]
        public bool IsCustomerAnonymous { get; set; }
        [DataMember]
        public string CustomerUsername { get; set; }
        [DataMember]
        public string CustomerEmailAddress { get; set; }
        [DataMember]
        public bool IsOrderSuccessful { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string OrderNo { get; set; }
        [DataMember]
        public DateTime OrderDateTime { get; set; }
    }
}
