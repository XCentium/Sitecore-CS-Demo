﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSDemo.Models.Product
{
    public class ComplementaryProductResult
    {
        [JsonProperty(PropertyName = "success")]
        public bool IsSuccessful { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public IEnumerable<string> Messages { get; set; }

        [JsonProperty(PropertyName = "result")]
        public IEnumerable<string> ProductIds { get; set; }
    }
}