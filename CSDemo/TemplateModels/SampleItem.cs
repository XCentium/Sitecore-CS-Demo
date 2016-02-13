using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class SampleItem : SitecoreItem, ISitecoreItem, ISampleItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}";
        
        public const string TitleFieldId = "{75577384-3C97-45DA-A847-81B00500E250}";
        
        public const string TitleFieldName = "Title";
        
        public const string TextFieldId = "{A60ACD61-A6DB-4182-8329-C957982CEC74}";
        
        public const string TextFieldName = "Text";
        #endregion
        
        #region Properties
[SitecoreItemField(TitleFieldId)] 
 public virtual string Title { get; set; } 

[SitecoreItemField(TextFieldId)] 
 public virtual string Text { get; set; } 
#endregion
        
    }
}
