using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCatalog : SitecoreItem, ISitecoreItem, ICommerceCatalog {
        
        #region Members
        public const string SitecoreItemTemplateId = "{93AF861A-B6F4-45BE-887D-D93D4B95B39D}";
        
        public const string WeightMeasureFieldId = "{6B61D05D-9B0F-4DAA-83A9-3DB57A1167A7}";
        
        public const string WeightMeasureFieldName = "WeightMeasure";
        
        public const string StartDateFieldId = "{FB9F19D1-D30D-4FE7-BD1D-4B6EB57A1BF6}";
        
        public const string StartDateFieldName = "StartDate";
        
        public const string EndDateFieldId = "{CC6C8787-19BE-4A7B-9A80-E476063056FB}";
        
        public const string EndDateFieldName = "EndDate";
        
        public const string VariantIdFieldId = "{BF35E3CC-B8FC-41C9-AFCD-A02034DB1B36}";
        
        public const string VariantIdFieldName = "VariantId";
        
        public const string ProductIdFieldId = "{B432CFBF-2F2B-4349-BF92-DD4A501816FB}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string DefaultLanguageFieldId = "{92388948-FAEE-4469-B7CC-61AFA3F495C2}";
        
        public const string DefaultLanguageFieldName = "DefaultLanguage";
        
        public const string CurrencyFieldId = "{4969C299-EF6F-4E55-ABD0-9200485793AD}";
        
        public const string CurrencyFieldName = "Currency";
        
        public const string CatalogNameFieldId = "{9EB95C6A-189D-4A5C-8152-78E1E79E402E}";
        
        public const string CatalogNameFieldName = "CatalogName";
        
        public const string DependentCatalogsFieldId = "{566F4A23-DEA1-4E13-9EA2-96E866D1C813}";
        
        public const string DependentCatalogsFieldName = "Dependent Catalogs";
        
        public const string ReportingLanguageFieldId = "{2BDDE9AF-D103-4B10-991E-701563EBAD8C}";
        
        public const string ReportingLanguageFieldName = "ReportingLanguage";
        #endregion
        
        #region Properties
[SitecoreItemField(WeightMeasureFieldId)] 
 public virtual string WeightMeasure { get; set; } 

[SitecoreItemField(StartDateFieldId)] 
 public virtual DateTime StartDate { get; set; } 

[SitecoreItemField(EndDateFieldId)] 
 public virtual DateTime EndDate { get; set; } 

[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(DefaultLanguageFieldId)] 
 public virtual string DefaultLanguage { get; set; } 

[SitecoreItemField(CurrencyFieldId)] 
 public virtual string Currency { get; set; } 

[SitecoreItemField(CatalogNameFieldId)] 
 public virtual string CatalogName { get; set; } 

[SitecoreItemField(DependentCatalogsFieldId)] 
 public virtual string DependentCatalogs { get; set; } 

[SitecoreItemField(ReportingLanguageFieldId)] 
 public virtual string ReportingLanguage { get; set; } 
#endregion
        
    }
}
