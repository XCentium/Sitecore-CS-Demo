using System.Collections.Generic;

namespace CSDemo.Models.Product
{
    public class VariantSize
    {
        public IEnumerable<VariantSizeLine> VariantSizeLines { get; set; }

    }
}