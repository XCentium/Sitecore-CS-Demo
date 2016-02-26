using XCore.Framework.ItemMapper;

namespace CSDemo.Contracts {
    
    
    public partial interface ISite : ISitecoreItem {
        
        #region Properties
string Tagline { get; set; } 

string PhoneNumber { get; set; } 

string PreTitle { get; set; } 

string email { get; set; } 

string Title { get; set; } 
#endregion
        
    }
}
