using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Services
{
    public class CinemaList
    {
        [SitecoreChildren]
        public IEnumerable<Cinema> Cinemas { get; set; }

        public string GetZipcodes()
        {
            var zipcodes = string.Empty;

            var cnt = 0;
            foreach (var cinema in Cinemas)
            {
                zipcodes += cnt > 0 ? "," : string.Empty;
                zipcodes += cinema.Zipcode;

                cnt++;
            }

            return zipcodes;
        }
    }
}