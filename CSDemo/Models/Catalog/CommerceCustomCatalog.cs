using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCustomCatalog : SitecoreItem, ISitecoreItem, ICommerceCustomCatalog {
        
        #region Members
        public const string SitecoreItemTemplateId = "{19988B71-6F3D-43EB-8253-E6D451878922}";
        
        public const string DefaultLanguageFieldId = "{22A0F611-C65C-44E7-A9F7-43CE85C451DE}";
        
        public const string DefaultLanguageFieldName = "DefaultLanguage";
        
        public const string CurrencyFieldId = "{171F44E4-02A4-473C-AC76-A1EB0002C7CF}";
        
        public const string CurrencyFieldName = "Currency";
        
        public const string WeightMeasureFieldId = "{03427B9B-6DE5-4EAE-8403-8F532FF6EC7C}";
        
        public const string WeightMeasureFieldName = "WeightMeasure";
        
        public const string ReportingLanguageFieldId = "{D3C3ECF7-D7C4-4174-A150-931306C90F9E}";
        
        public const string ReportingLanguageFieldName = "ReportingLanguage";
        
        public const string EndDateFieldId = "{27414822-E3BB-4014-B434-0066AEE9D86A}";
        
        public const string EndDateFieldName = "EndDate";
        
        public const string MaterializeFieldId = "{01D6D4E8-C90A-4C74-B92D-31F7B483D94A}";
        
        public const string MaterializeFieldName = "Materialize";
        
        public const string CatalogNameFieldId = "{F957A725-5055-4E2F-9693-AD340D7F14F9}";
        
        public const string CatalogNameFieldName = "CatalogName";
        
        public const string SourceCatalogsFieldId = "{48F6BBB0-F5EF-4693-ACB5-3614F49C5725}";
        
        public const string SourceCatalogsFieldName = "Source Catalogs";
        
        public const string StartDateFieldId = "{41B4C5A3-D7D1-4DAB-88B2-92BC5E73726E}";
        
        public const string StartDateFieldName = "StartDate";
        
        public const string DependentCatalogsFieldId = "{6DBB8FBC-4D13-480A-B717-2DA8F6ECC67A}";
        
        public const string DependentCatalogsFieldName = "Dependent Catalogs";
        #endregion
        
        #region Properties
[SitecoreItemField(DefaultLanguageFieldId)] 
 public virtual string DefaultLanguage { get; set; } 

[SitecoreItemField(CurrencyFieldId)] 
 public virtual string Currency { get; set; } 

[SitecoreItemField(WeightMeasureFieldId)] 
 public virtual string WeightMeasure { get; set; } 

[SitecoreItemField(ReportingLanguageFieldId)] 
 public virtual string ReportingLanguage { get; set; } 

[SitecoreItemField(EndDateFieldId)] 
 public virtual DateTime EndDate { get; set; } 

[SitecoreItemField(MaterializeFieldId)] 
 public virtual bool Materialize { get; set; } 

[SitecoreItemField(CatalogNameFieldId)] 
 public virtual string CatalogName { get; set; } 

[SitecoreItemField(SourceCatalogsFieldId)] 
 public virtual string SourceCatalogs { get; set; } 

[SitecoreItemField(StartDateFieldId)] 
 public virtual DateTime StartDate { get; set; } 

[SitecoreItemField(DependentCatalogsFieldId)] 
 public virtual string DependentCatalogs { get; set; } 
#endregion
        
    }
}
