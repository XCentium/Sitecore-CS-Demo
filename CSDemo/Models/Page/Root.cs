#region

using CSDemo.Contracts.Page;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using Sitecore.Data.Items;
using CSDemo.Models.Account;

#endregion

namespace CSDemo.Models.Page
{
    [SitecoreType]
    public class Root : IRoot
    {
        public Root()
        {
            UserStatus = UserStatus.GetStatus(Sitecore.Context.User);
        }

        #region Properties

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public virtual string Name { get; set; }

        [SitecoreField(Fields.Title)]
        public virtual string Title { get; set; }

        [SitecoreField(Fields.Email)]
        public virtual string Email { get; set; }

        [SitecoreField(Fields.PreTitle)]
        public virtual string PreTitle { get; set; }

        [SitecoreField(Fields.Tagline)]
        public virtual string Tagline { get; set; }

        [SitecoreField(Fields.PhoneNumber)]
        public virtual string PhoneNumber { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        [SitecoreIgnore]
        public UserStatus UserStatus { get; set; }

        [SitecoreField(Fields.Catalog)]
        public virtual Item Catalog { get; set; }

        #endregion


        #region Fields
        public struct Fields
        {
            public const string Title = "Title";
            public const string Email = "email";
            public const string PreTitle = "PreTitle";
            public const string Tagline = "Tagline";
            public const string PhoneNumber = "Phone Number";
            public const string Catalog = "Catalog";
        }
        #endregion
    }
}