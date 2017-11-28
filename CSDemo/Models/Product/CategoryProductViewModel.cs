using System.Collections.Generic;

namespace CSDemo.Models.Product
{
    public class CategoryProductViewModel
    {
        public Category Category { get; set; }
        public string ParentCategory { get; set; }

        public IEnumerable<CategoryMenulistViewModel> CategoryMenulist { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; }
    }
}