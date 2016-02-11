using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Fields {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceField : SitecoreItem, ISitecoreItem, ICommerceField {
        
        #region Members
        public const string SitecoreItemTemplateId = "{6A553EC0-2AB9-49D6-BE0E-A472E5E24213}";
        
        public const string IsRequiredFieldId = "{63CB4F24-A2D2-4082-9821-8F350A781E48}";
        
        public const string IsRequiredFieldName = "Is Required";
        
        public const string DisplayOnSiteFieldId = "{FF45E279-7051-42AD-BF91-117F5EC4BD16}";
        
        public const string DisplayOnSiteFieldName = "Display On Site";
        
        public const string SpecificationSearchableFieldId = "{7EE38888-5F13-4CB1-BAD0-09F5B7759856}";
        
        public const string SpecificationSearchableFieldName = "Specification Searchable";
        
        public const string FreeTextSearchableFieldId = "{81180D30-1185-426F-AE2D-7DA7741399A1}";
        
        public const string FreeTextSearchableFieldName = "Free Text Searchable";
        
        public const string BasePropertyFieldId = "{D8A78E27-F87F-409B-A4DF-D69335DD466F}";
        
        public const string BasePropertyFieldName = "Base Property";
        
        public const string AssignToAllProductTypesFieldId = "{A0F35A32-14B5-4147-9851-7E0BBDC17EFC}";
        
        public const string AssignToAllProductTypesFieldName = "Assign To All Product Types";
        #endregion
        
        #region Properties
[SitecoreItemField(IsRequiredFieldId)] 
 public virtual bool IsRequired { get; set; } 

[SitecoreItemField(DisplayOnSiteFieldId)] 
 public virtual bool DisplayOnSite { get; set; } 

[SitecoreItemField(SpecificationSearchableFieldId)] 
 public virtual bool SpecificationSearchable { get; set; } 

[SitecoreItemField(FreeTextSearchableFieldId)] 
 public virtual bool FreeTextSearchable { get; set; } 

[SitecoreItemField(BasePropertyFieldId)] 
 public virtual bool BaseProperty { get; set; } 

[SitecoreItemField(AssignToAllProductTypesFieldId)] 
 public virtual bool AssignToAllProductTypes { get; set; } 
#endregion
        
    }
}
