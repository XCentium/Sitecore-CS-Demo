using Glass.Mapper.Sc.Configuration.Attributes;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CSDemo.Models.Store
{
    [SitecoreType(AutoMap = true)]
    public class Store : LocationInformation
    {

        public virtual string Title { get; set; }



        #region Methods

        // Distance is measured in millimeters
        public static IEnumerable<Store> SortByProximity(LocationInformation origin, IEnumerable<Store> destinations)
        {
            var response = string.Empty;
            Log.Info("Sorting started", origin);
            try
            {
                var url = string.Format(Constants.Store.GoogleLocationMatrixApiUrl);
                var query = $"origins={origin.Latitude},{origin.Longitude}&destinations={string.Join("|", destinations.Select(d=> !string.IsNullOrWhiteSpace(d.Latitude.ToString()) && !string.IsNullOrWhiteSpace(d.Longitude.ToString()) ? $"{d.Latitude},{d.Longitude}": $"{d.Street} {d.City} {d.State} {d.Zip}"))}";
                var syncClient = new WebClient();
                Log.Info($"{url}?{query}", syncClient);
                response = syncClient.DownloadString($"{url}?{query}");
                Log.Info("Google response: "+response, syncClient);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to get coors from Google: "+ex.Message, ex);
            }

            if (string.IsNullOrEmpty(response)) return null;

            var result = JsonConvert.DeserializeObject<dynamic>(response);
            if (!result.IsSuccessful || result.status!="OK")
            {
                Log.Error("Unable to calculate distance to stores.", result);
                 return null;
            }
            Log.Info("Pulling result object from Google response", origin);
            var distances = new Dictionary<Store, int>();
            foreach (var row in result.rows)
            {
                var elements = row.elements;
                var count = 0;
                foreach(var element in elements)
                {
                    count++;
                    var distance = element.distance;
                    var destination = destinations.Skip(count - 1).Take(1).FirstOrDefault();
                    distances.Add(destination, distance.Value);
                }
            }
            return distances.OrderByDescending(i=>i.Value).Select(i=>i.Key);
        }

        #endregion
    }
}