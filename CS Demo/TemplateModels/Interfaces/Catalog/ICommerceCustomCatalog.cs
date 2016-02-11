using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceCustomCatalog : ISitecoreItem {
        
        #region Properties
string DefaultLanguage { get; set; } 

string Currency { get; set; } 

string WeightMeasure { get; set; } 

string ReportingLanguage { get; set; } 

DateTime EndDate { get; set; } 

bool Materialize { get; set; } 

string CatalogName { get; set; } 

string SourceCatalogs { get; set; } 

DateTime StartDate { get; set; } 

string DependentCatalogs { get; set; } 
#endregion
        
    }
}
