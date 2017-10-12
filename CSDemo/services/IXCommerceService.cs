using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CSDemo.Services
{
    [ServiceContract]
    public interface IXCommerceService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        MovieOrder BuyMovie(MovieOrder order);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<Movie> GetShowTimes(string zipcode, int hours);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<Movie> GetRecommendationsByUser(string userEmailAddress);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<Movie> GetRecommendationsByMovie(string movieVariantId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<Movie> GetRecommendationsByRatings();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetMovieDescription(string movieVariantId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        GetMovieTwitterSentimentsResponse GetMovieTwitterSentiments(string movieVariantId);
    }
}