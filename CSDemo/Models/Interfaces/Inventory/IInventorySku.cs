using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Inventory {
    
    
    public partial interface IInventorySku : ISitecoreItem {
        
        #region Properties
string SkuLastModified { get; set; } 

string ExcessOnHandQuantity { get; set; } 

DateTime BackorderAvailabilityDate { get; set; } 

string OnHandQuantity { get; set; } 

string PreorderedQuantity { get; set; } 

string PreorderLimit { get; set; } 

string ProductCatalogName { get; set; } 

DateTime LastRestocked { get; set; } 

string StockOutThreshold { get; set; } 

string BackorderLimit { get; set; } 

string ReorderPoint { get; set; } 

string InventoryCatalogName { get; set; } 

string BackorderedQuantity { get; set; } 

bool Backorderable { get; set; } 

string UnitOfMeasure { get; set; } 

string SkuVariantId { get; set; } 

string SkuId { get; set; } 

string TargetQuantity { get; set; } 

string Memo { get; set; } 

bool Preorderable { get; set; } 

DateTime PreorderAvailabilityDate { get; set; } 

string Status { get; set; } 
#endregion
        
    }
}
