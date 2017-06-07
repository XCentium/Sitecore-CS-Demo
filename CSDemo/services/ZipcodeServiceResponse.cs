using Newtonsoft.Json;

namespace CSDemo.Services
{
    public class ZipcodeServiceResponse
    {
        [JsonProperty("zip_code1")]
        public string ZipCode1 { get; set; }
        [JsonProperty("zip_code2")]
        public string ZipCode2 { get; set; }
        [JsonProperty("distance")]
        public double Distance { get; set; }
    }
}