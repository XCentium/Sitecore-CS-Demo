using System.Collections.Generic;
using CSDemo.Contracts.GeneralSearch;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;


namespace CSDemo.Models.GeneralSearch
{

    public class SearchResult : ISearchResult
    {
        #region Properties

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Title { get; set; }
        
        [SitecoreField(Product.Product.Fields.Images)]
        public virtual IEnumerable<Image> Images { get; set; }

        [SitecoreField(Product.Product.Fields.Price)]
        public virtual decimal Price { get; set; }
        
        [SitecoreField(Product.Product.Fields.OnSale)]
        public virtual bool IsOnSale { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

        public virtual bool IsNew { get; set; }

        public virtual double Rating { get; set; }

        

        #endregion

    }
}