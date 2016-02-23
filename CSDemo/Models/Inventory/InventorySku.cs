using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Inventory {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class InventorySku : SitecoreItem, ISitecoreItem, IInventorySku {
        
        #region Members
        public const string SitecoreItemTemplateId = "{A42418BC-E80E-4DF6-BB19-9E60D6A4D346}";
        
        public const string SkuLastModifiedFieldId = "{BB7EE852-3F16-41D0-A36E-4EFA68C76295}";
        
        public const string SkuLastModifiedFieldName = "SkuLastModified";
        
        public const string ExcessOnHandQuantityFieldId = "{845F5AC0-BA27-4B13-B94B-1745C699ABA3}";
        
        public const string ExcessOnHandQuantityFieldName = "ExcessOnHandQuantity";
        
        public const string BackorderAvailabilityDateFieldId = "{1A4F63ED-19DA-4E0A-8525-DF0B1C6B71BD}";
        
        public const string BackorderAvailabilityDateFieldName = "BackorderAvailabilityDate";
        
        public const string OnHandQuantityFieldId = "{169DC396-219E-4A84-80F2-BA9ADB82212C}";
        
        public const string OnHandQuantityFieldName = "OnHandQuantity";
        
        public const string PreorderedQuantityFieldId = "{72526237-01A6-45DE-B5D9-51B733255270}";
        
        public const string PreorderedQuantityFieldName = "PreorderedQuantity";
        
        public const string PreorderLimitFieldId = "{7EA12213-E07E-444A-B1FE-B2D7B06E6ED6}";
        
        public const string PreorderLimitFieldName = "PreorderLimit";
        
        public const string ProductCatalogNameFieldId = "{4DD2FDC6-188E-4CD0-A282-6D2FF8F46AB1}";
        
        public const string ProductCatalogNameFieldName = "ProductCatalogName";
        
        public const string LastRestockedFieldId = "{9E409002-80E9-427C-98DE-0C5F0B5CBC10}";
        
        public const string LastRestockedFieldName = "LastRestocked";
        
        public const string StockOutThresholdFieldId = "{5EA258FD-3D23-4361-9B5F-0BDE61C1BBB3}";
        
        public const string StockOutThresholdFieldName = "StockOutThreshold";
        
        public const string BackorderLimitFieldId = "{4615C92E-F355-4FA0-8B0C-42C107C8D9F9}";
        
        public const string BackorderLimitFieldName = "BackorderLimit";
        
        public const string ReorderPointFieldId = "{8238DAD1-CE37-4A5A-A0CD-C3F0E1967AA3}";
        
        public const string ReorderPointFieldName = "ReorderPoint";
        
        public const string InventoryCatalogNameFieldId = "{ABD95DE7-88B3-43A9-85D5-F7FB7BE56689}";
        
        public const string InventoryCatalogNameFieldName = "InventoryCatalogName";
        
        public const string BackorderedQuantityFieldId = "{D4A6C265-CFA4-4CD4-9B81-4CB1CF782C6A}";
        
        public const string BackorderedQuantityFieldName = "BackorderedQuantity";
        
        public const string BackorderableFieldId = "{88CF95B9-DBC7-47FD-8779-B02ABCDA18E9}";
        
        public const string BackorderableFieldName = "Backorderable";
        
        public const string UnitOfMeasureFieldId = "{915A06DB-C68C-4D61-A168-F73764727885}";
        
        public const string UnitOfMeasureFieldName = "UnitOfMeasure";
        
        public const string SkuVariantIdFieldId = "{165DEAC7-8CF9-47E9-8D43-353FBF4E0364}";
        
        public const string SkuVariantIdFieldName = "SkuVariantId";
        
        public const string SkuIdFieldId = "{56F6623E-FFAB-498A-99AC-943E5886E05D}";
        
        public const string SkuIdFieldName = "SkuId";
        
        public const string TargetQuantityFieldId = "{E57A3FD4-5A6C-4CBD-8094-0ECDB63190F6}";
        
        public const string TargetQuantityFieldName = "TargetQuantity";
        
        public const string MemoFieldId = "{A9942BCB-8D52-43E6-83D7-6ECE87160318}";
        
        public const string MemoFieldName = "Memo";
        
        public const string PreorderableFieldId = "{C1AD0EB0-5FBB-4001-A692-659B1236B869}";
        
        public const string PreorderableFieldName = "Preorderable";
        
        public const string PreorderAvailabilityDateFieldId = "{0A961DF9-AF1B-4AB5-A453-2DD2B18C3BBB}";
        
        public const string PreorderAvailabilityDateFieldName = "PreorderAvailabilityDate";
        
        public const string StatusFieldId = "{D646EFFA-82B4-4722-9C51-DD32CEA0CD66}";
        
        public const string StatusFieldName = "Status";
        #endregion
        
        #region Properties
[SitecoreItemField(SkuLastModifiedFieldId)] 
 public virtual string SkuLastModified { get; set; } 

[SitecoreItemField(ExcessOnHandQuantityFieldId)] 
 public virtual string ExcessOnHandQuantity { get; set; } 

[SitecoreItemField(BackorderAvailabilityDateFieldId)] 
 public virtual DateTime BackorderAvailabilityDate { get; set; } 

[SitecoreItemField(OnHandQuantityFieldId)] 
 public virtual string OnHandQuantity { get; set; } 

[SitecoreItemField(PreorderedQuantityFieldId)] 
 public virtual string PreorderedQuantity { get; set; } 

[SitecoreItemField(PreorderLimitFieldId)] 
 public virtual string PreorderLimit { get; set; } 

[SitecoreItemField(ProductCatalogNameFieldId)] 
 public virtual string ProductCatalogName { get; set; } 

[SitecoreItemField(LastRestockedFieldId)] 
 public virtual DateTime LastRestocked { get; set; } 

[SitecoreItemField(StockOutThresholdFieldId)] 
 public virtual string StockOutThreshold { get; set; } 

[SitecoreItemField(BackorderLimitFieldId)] 
 public virtual string BackorderLimit { get; set; } 

[SitecoreItemField(ReorderPointFieldId)] 
 public virtual string ReorderPoint { get; set; } 

[SitecoreItemField(InventoryCatalogNameFieldId)] 
 public virtual string InventoryCatalogName { get; set; } 

[SitecoreItemField(BackorderedQuantityFieldId)] 
 public virtual string BackorderedQuantity { get; set; } 

[SitecoreItemField(BackorderableFieldId)] 
 public virtual bool Backorderable { get; set; } 

[SitecoreItemField(UnitOfMeasureFieldId)] 
 public virtual string UnitOfMeasure { get; set; } 

[SitecoreItemField(SkuVariantIdFieldId)] 
 public virtual string SkuVariantId { get; set; } 

[SitecoreItemField(SkuIdFieldId)] 
 public virtual string SkuId { get; set; } 

[SitecoreItemField(TargetQuantityFieldId)] 
 public virtual string TargetQuantity { get; set; } 

[SitecoreItemField(MemoFieldId)] 
 public virtual string Memo { get; set; } 

[SitecoreItemField(PreorderableFieldId)] 
 public virtual bool Preorderable { get; set; } 

[SitecoreItemField(PreorderAvailabilityDateFieldId)] 
 public virtual DateTime PreorderAvailabilityDate { get; set; } 

[SitecoreItemField(StatusFieldId)] 
 public virtual string Status { get; set; } 
#endregion
        
    }
}
