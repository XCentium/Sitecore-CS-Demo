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

            try
            {
                var url = string.Format(Constants.Store.GoogleLocationMatrixApiUrl);
                var query = $"origins={origin.Latitude},{origin.Longitude}&destinations={string.Join("|", destinations.Select(d=> !string.IsNullOrWhiteSpace(d.Latitude.ToString()) && !string.IsNullOrWhiteSpace(d.Longitude.ToString()) ? $"{d.Latitude},{d.Longitude}": $"{d.Street} {d.City} {d.State} {d.Zip}"))}";
                var syncClient = new WebClient();
                response = syncClient.DownloadString($"{url}?{query}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

            if (string.IsNullOrEmpty(response)) return null;

            var result = JsonConvert.DeserializeObject<dynamic>(response);
            if (!result.IsSuccessful || result.status!="OK")
            {
                Log.Error("Unable to calculate distance to stores.", result);
                 return null;
            }
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