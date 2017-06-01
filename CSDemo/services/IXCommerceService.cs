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
    }
}