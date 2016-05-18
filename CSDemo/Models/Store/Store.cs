﻿using Glass.Mapper.Sc.Configuration.Attributes;
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
                var query = $"origins={origin.Latitude},{origin.Longitude}&destinations={string.Join("|", destinations.Select(d => !string.IsNullOrWhiteSpace(d.Latitude.ToString()) && !string.IsNullOrWhiteSpace(d.Longitude.ToString()) ? $"{d.Latitude},{d.Longitude}" : $"{d.Street} {d.City} {d.State} {d.Zip}"))}";
                var syncClient = new WebClient();
                response = syncClient.DownloadString($"{url}?{query}");
            }
            catch (Exception ex)
            {
                Log.Error("Unable to get coors from Google: " + ex.Message, ex);
            }

            if (string.IsNullOrEmpty(response)) return null;
            
            var result = JsonConvert.DeserializeObject<dynamic>(response);
            if (result==null || result.status!="OK")
            {
                Log.Error("Unable to calculate distance to stores.", result);
                 return null;
            }
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