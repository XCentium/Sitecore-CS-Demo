using System.Collections.Generic;

namespace CSDemo.Models.Product
{
    public class CategoryMenulistViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }

        public string Url { get; set; }
        public IEnumerable<ProductMenulistViewModel> ProductMenulistViewModel { get; set; }

        public int ProductsCount { get; set; }

    }
}