using System;
using System.Collections.Generic;
using CSDemo.Contracts;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true, TemplateId = "{4DC5C4F1-59EA-46B1-AB21-D7F98CF80595}")]
    public class ProductGroups
    {
        [SitecoreChildren]
        public virtual IEnumerable<ProductGroup> Groups { get; set; }
    }

    [SitecoreType(AutoMap = true, TemplateId = "{491D4F25-95CE-43AC-897C-51981F6864BD}")]
    public class ProductGroup: IEditableBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [SitecoreField("Product Categories")]
        public IEnumerable<Category> ProductCategories { get; set; }

        public Guid ID { get; set; }
        public string Path { get; set; }
    }
}