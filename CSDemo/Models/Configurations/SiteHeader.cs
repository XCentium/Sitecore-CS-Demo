using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Configurations {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class SiteHeader : SitecoreItem, ISitecoreItem, ISiteHeader {
        
        #region Members
        public const string SitecoreItemTemplateId = "{3716B095-9C55-45C8-BB60-08EACF39CDA3}";
        
        public const string TaglineFieldId = "{8254F343-7298-4618-9167-F850519E7E5D}";
        
        public const string TaglineFieldName = "Tagline";
        
        public const string TitleFieldId = "{FB1F3432-D97F-4168-83E9-DA4FAFA53065}";
        
        public const string TitleFieldName = "Title";
        
        public const string PreTitleFieldId = "{96A811BA-4AAB-4005-A75F-AA2A242941E2}";
        
        public const string PreTitleFieldName = "PreTitle";
        
        public const string PhoneNumberFieldId = "{39FD34CC-0DAF-49FE-B103-70F7BF36A65F}";
        
        public const string PhoneNumberFieldName = "Phone Number";
        
        public const string emailFieldId = "{6488FE35-2E12-4396-A26D-7180D3701450}";
        
        public const string emailFieldName = "email";
        #endregion
        
        #region Properties
[SitecoreItemField(TaglineFieldId)] 
 public virtual string Tagline { get; set; } 

[SitecoreItemField(TitleFieldId)] 
 public virtual string Title { get; set; } 

[SitecoreItemField(PreTitleFieldId)] 
 public virtual string PreTitle { get; set; } 

[SitecoreItemField(PhoneNumberFieldId)] 
 public virtual string PhoneNumber { get; set; } 

[SitecoreItemField(emailFieldId)] 
 public virtual string email { get; set; } 
#endregion
        
    }
}
