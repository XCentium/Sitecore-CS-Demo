using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class VariantColor
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public IEnumerable<VariantColorLine> VariantColorLines { get; set; }
    }
}