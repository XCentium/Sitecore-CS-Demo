using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class CategoryProductViewModel
    {
        public Category Category { get; set; }

        public IEnumerable<CategoryMenulistViewModel> CategoryMenulist { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; }
    }
}