using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using CSDemo.Helpers;
using CSDemo.Models.Product;

namespace CSDemo.AJAX
{
    /// <summary>
    /// Summary description for Products
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Products : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<ProductMini> GetProductRecommendations(string productId, string variantId, int numberOfResults = 10)
        {
            return RecommendationsHelper.GetItemRecommendations(productId, variantId, numberOfResults);
        }
    }
}
