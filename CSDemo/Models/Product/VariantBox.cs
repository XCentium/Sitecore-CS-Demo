using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class VariantBox
    {
        public IEnumerable<VariantBoxLine> VariantBoxLines { get; set; }

    }
         
}