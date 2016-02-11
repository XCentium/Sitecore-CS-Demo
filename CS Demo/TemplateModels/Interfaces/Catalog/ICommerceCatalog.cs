using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceCatalog : ISitecoreItem {
        
        #region Properties
string WeightMeasure { get; set; } 

DateTime StartDate { get; set; } 

DateTime EndDate { get; set; } 

string VariantId { get; set; } 

string ProductId { get; set; } 

string DefaultLanguage { get; set; } 

string Currency { get; set; } 

string CatalogName { get; set; } 

string DependentCatalogs { get; set; } 

string ReportingLanguage { get; set; } 
#endregion
        
    }
}
