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
            //try
            //{
            //    var url = string.Format(Constants.Store.GoogleLocationMatrixApiUrl);
            //    var query = $"origins={origin.Latitude},{origin.Longitude}&destinations={string.Join("|", destinations.Select(d=> !string.IsNullOrWhiteSpace(d.Latitude.ToString()) && !string.IsNullOrWhiteSpace(d.Longitude.ToString()) ? $"{d.Latitude},{d.Longitude}": $"{d.Street} {d.City} {d.State} {d.Zip}"))}";
            //    var syncClient = new WebClient();
            //    Log.Info($"{url}?{query}", syncClient);
            //    response = syncClient.DownloadString($"{url}?{query}");
            //    Log.Info("Google response: "+response, syncClient);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Unable to get coors from Google: "+ex.Message, ex);
            //}
            response = "{\"destination_addresses\":[\"6762 N Glenwood St, Boise, ID 83714, USA\",\"8900 Viscount Blvd, El Paso, TX 79925, USA\",\"4275 E Charleston Blvd, Las Vegas, NV 89104, USA\",\"14014 NW Passage, Marina Del Rey, CA 90292, USA\"],\"origin_addresses\":[\"24514 Town Center Dr, Santa Clarita, CA 91355, USA\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"1,320 km\",\"value\":1319829},\"duration\":{\"text\":\"12 hours 56 mins\",\"value\":46550},\"status\":\"OK\"},{\"distance\":{\"text\":\"1,358 km\",\"value\":1357536},\"duration\":{\"text\":\"12 hours 8 mins\",\"value\":43679},\"status\":\"OK\"},{\"distance\":{\"text\":\"450 km\",\"value\":449718},\"duration\":{\"text\":\"4 hours 23 mins\",\"value\":15761},\"status\":\"OK\"},{\"distance\":{\"text\":\"64.4 km\",\"value\":64446},\"duration\":{\"text\":\"53 mins\",\"value\":3156},\"status\":\"OK\"}]}],\"status\":\"OK\"}";
            if (string.IsNullOrEmpty(response)) return null;
            
            var result = JsonConvert.DeserializeObject<dynamic>(response);
            if (result==null || result.status!="OK")
            {
                Log.Error("Unable to calculate distance to stores.", result);
                 return null;
            }
            Log.Info("Pulling result object from Google response", origin);
            var distances = new Dictionary<Store, int>();
            foreach (var row in result.rows)
            {
                if (row == null) continue;
                var elements = row.elements;
                var count = 0;
                foreach (var element in elements)
                {
                    count++;
                    if (element == null) continue;
                    foreach (var estimate in element)
                    {
                        if (estimate == null || estimate.Name != "distance") continue;
                        var distance = estimate.First;
                        if (distance == null) continue;
                        var destination = destinations.Skip(count - 1).First();
                        distances.Add(destination, (int)distance.value);
                    }
                }
            }
            return distances.OrderBy(i=>i.Value).Select(i=>i.Key);
        }

        #endregion
    }
}