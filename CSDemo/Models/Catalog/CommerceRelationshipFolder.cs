using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceRelationshipFolder : SitecoreItem, ISitecoreItem, ICommerceRelationshipFolder {
        
        #region Members
        public const string SitecoreItemTemplateId = "{4810C97B-5265-4D94-B122-642402169067}";
        
        public const string RelationshipNameFieldId = "{5579E24D-4264-4F79-B060-06FC156E04AE}";
        
        public const string RelationshipNameFieldName = "RelationshipName";
        #endregion
        
        #region Properties
[SitecoreItemField(RelationshipNameFieldId)] 
 public virtual string RelationshipName { get; set; } 
#endregion
        
    }
}
