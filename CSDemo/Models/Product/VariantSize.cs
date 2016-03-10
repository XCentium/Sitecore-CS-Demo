using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class VariantSize
    {
        public IEnumerable<VariantSizeLine> VariantSizeLines { get; set; }

    }
}