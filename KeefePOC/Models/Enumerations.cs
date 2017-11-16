using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Models.Enumerations
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProgramType
    {
        Doc,
        Hospital,
        Jail,
        Other
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RestrictionType
    {
        Weight,
        Price
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FacilityType
    {
        Jail,
        Hospital,
        Other
    }
}
