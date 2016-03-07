using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class CategoryMenulistViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductMenulistViewModel> ProductMenulistViewModel { get; set; }

        public int ProductsCount { get; set; }
        
    }
}