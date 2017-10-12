using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSDemo.Models.Product;
using LinqToTwitter;
using Newtonsoft.Json;

namespace CSDemo.Services
{
    public class TextAnalysisApi
    {
        public static GetMovieTwitterSentimentsResponse GetMovieTwitterSentiments(string movieVariantId)
        {
            var response = new GetMovieTwitterSentimentsResponse();

            var searchTerm = ProductHelper.GetMovieSearchTerm(movieVariantId);

            if (string.IsNullOrWhiteSpace(searchTerm)) return response;
            response.SearchTerm = searchTerm;

            var demoTask = DoDemosAsync(response, searchTerm);
            demoTask.Wait();

            return response;
        }

        static async Task DoDemosAsync(GetMovieTwitterSentimentsResponse response, string searchTerm)
        {
            var auth = DoApplicationOnlyAuth();

            await auth.AuthorizeAsync();

            var twitterCtx = new TwitterContext(auth);
            await SearchDemoRunAsync(twitterCtx, response, searchTerm);
        }

        static IAuthorizer DoApplicationOnlyAuth()
        {
            var auth = new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"]
                },
            };

            return auth;
        }

        internal static async Task SearchDemoRunAsync(TwitterContext twitterCtx, GetMovieTwitterSentimentsResponse getMovieTwitterSentimentsResponse, string searchTerm)
        {
            //const string searchTerm = "#ItMovie2017";
            ////const string searchTerm = "#AmericanMade";
            ////const string searchTerm = "#AmericanAssassin";

            Console.WriteLine($"------ MOVIE ({searchTerm}) ------");

            var searchResponse =
                await
                    (from search in twitterCtx.Search
                        where search.Type == SearchType.Search &&
                              search.Query == searchTerm &&
                              search.ResultType == ResultType.Mixed &&
                              search.Count == 200 &&
                              search.SearchLanguage == "en"
                        select search)
                    .SingleOrDefaultAsync();

            if (searchResponse?.Statuses != null)
            {
                var tweets = searchResponse.Statuses.Where(s => s.Text.StartsWith("RT ") == false).ToList(); //remove RT's

                getMovieTwitterSentimentsResponse.TotalTweetCount = tweets.Count;

                 //get tweets analyzed
                 var request = new TextAnalyticsDocument
                {
                    Documents = GetAnalyticsDocuments(tweets)
                };

                var response = GetSentimentAnalysis(request);
                var average = response.Documents.Average(d => d.Score);

                getMovieTwitterSentimentsResponse.AverageScore = average;
            }
        }

        private static TextAnalyticsDocument GetSentimentAnalysis(TextAnalyticsDocument request)
        {
            var doc = new TextAnalyticsDocument();

            if (request?.Documents != null && request.Documents.Count > 0)
            {
                const string url = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";

                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("Ocp-Apim-Subscription-Key", "a299a62a22884eb08792de1612d91b65");

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    var jsonData = JsonConvert.SerializeObject(request);

                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                }

                var response = (HttpWebResponse)webRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();

                    doc = JsonConvert.DeserializeObject<TextAnalyticsDocument>(responseText);
                }
            }

            return doc;
        }

        private static List<TextAnalyticsComment> GetAnalyticsDocuments(List<Status> tweets)
        {
            var docs = new List<TextAnalyticsComment>();
            var count = 1;

            if (tweets != null)
            {
                foreach (var tweet in tweets)
                {
                    docs.Add(new TextAnalyticsComment
                    {
                        Text = tweet.Text,
                        Id = count.ToString()
                    });

                    count++;
                }
            }

            return docs;
        }
    }

    public class TextAnalyticsDocument
    {
        public List<TextAnalyticsComment> Documents { get; set; }
        public List<TextAnalyticsError> Errors { get; set; }
    }

    public class TextAnalyticsComment
    {
        public string Language => "en";
        public string Id { get; set; }
        public string Text { get; set; }
        public decimal Score { get; set; }
    }

    public class TextAnalyticsError
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }

    public class GetMovieTwitterSentimentsResponse
    {
        public int TotalTweetCount { get; set; }
        public decimal AverageScore { get; set; }
        public string SearchTerm { get; set; }
    }
}