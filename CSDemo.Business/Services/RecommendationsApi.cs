using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CSDemo.Business.Services
{
    public class RecommendationsApi
    {
        private readonly HttpClient _httpClient;

        public string BaseUri { get; set; }

        public RecommendationsApi(string accountKey, string baseUri)
        {
            if (string.IsNullOrWhiteSpace(accountKey) || string.IsNullOrWhiteSpace(baseUri))
                throw new ArgumentException("Must supply Account Key and Base URI");

            BaseUri = baseUri;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUri),
                Timeout = TimeSpan.FromMinutes(15),
                DefaultRequestHeaders =
                {
                    {"Ocp-Apim-Subscription-Key", accountKey}
                }
            };
        }

        /// <summary>
        /// Get Item to Item (I2I) Recommendations or Frequently-Bought-Together (FBT) recommendations
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="buildId">The build identifier. Set to null if you want to use active build</param>
        /// <param name="itemIds"></param>
        /// <param name="numberOfResults"></param>
        /// <returns>
        /// The recommendation sets. Note that I2I builds will only return one item per set.
        /// FBT builds will return more than one item per set.
        /// </returns>
        public RecommendedItemSetInfoList GetRecommendations(string modelId, string buildId, string itemIds, int numberOfResults)
        {

            var uri = BaseUri + "/models/" + modelId + "/recommend/item?itemIds=" + itemIds +
                      "&numberOfResults=" + numberOfResults + "&minimalScore=0";

            //Set active build if passed.
            if (buildId != null)
            {
                uri = uri + "&buildId=" + buildId;
            }

            var response = _httpClient.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to get recommendations for modelId {modelId}, buildId {buildId}, Reason: {ExtractErrorInfo(response)}");
            }

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var recommendedItemSetInfoList = JsonConvert.DeserializeObject<RecommendedItemSetInfoList>(jsonString);

            return recommendedItemSetInfoList;
        }

        /// <summary>
        /// Use historical transaction data to provide personalized recommendations for a user.
        /// The user history is extracted from the usage files used to train the model.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="buildId">The build identifier. Set to null to use active build.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="numberOfResults">Desired number of recommendation results.</param>
        /// <returns>The recommendations for the user.</returns>
        public RecommendedItemSetInfoList GetUserRecommendations(string modelId, string buildId, string userId, int numberOfResults)
        {
            var uri = BaseUri + "/models/" + modelId + "/recommend/user?userId=" + userId + "&numberOfResults=" + numberOfResults;

            //Set active build if passed.
            if (buildId != null)
            {
                uri = uri + "&buildId=" + buildId;
            }

            var response = _httpClient.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to get user recommendations for modelId {modelId}, buildId {buildId}, Reason: {ExtractErrorInfo(response)}");
            }

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var recommendedItemSetInfoList = JsonConvert.DeserializeObject<RecommendedItemSetInfoList>(jsonString);

            return recommendedItemSetInfoList;
        }

        /// <summary>
        /// Extract error message from the httpResponse, (reason phrase + body)
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ExtractErrorInfo(HttpResponseMessage response)
        {
            string detailedReason = null;
            if (response.Content != null)
            {
                detailedReason = response.Content.ReadAsStringAsync().Result;
            }
            var errorMsg = detailedReason == null ? response.ReasonPhrase : response.ReasonPhrase + "->" + detailedReason;
            return errorMsg;

        }
    }

    [DataContract]
    public class RecommendedItemSetInfoList
    {
        [DataMember]
        [JsonProperty("recommendedItems")]
        public IEnumerable<RecommendedItemSetInfo> RecommendedItemSetInfo { get; set; }
    }

    /// <summary>
    /// Holds a recommendation result, which is a set of recommended items with reasoning and rating/score.
    /// </summary>
    [DataContract]
    public class RecommendedItemSetInfo
    {
        public RecommendedItemSetInfo()
        {
            Items = new List<RecommendedItemInfo>();
        }

        [DataMember]
        [JsonProperty("items")]
        public IEnumerable<RecommendedItemInfo> Items { get; set; }

        [DataMember]
        [JsonProperty("rating")]
        public double Rating { get; set; }

        [DataMember]
        [JsonProperty("reasoning")]
        public IEnumerable<string> Reasoning { get; set; }
    }

    [DataContract]
    public class RecommendedItemInfo
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }
}
