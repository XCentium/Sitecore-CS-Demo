using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Models.Blog
{
    public class BlogAuthor
    {
        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]

        public virtual string Name { get; set; }
        [SitecoreField(Fields.Title)]

        public virtual string Title { get; set; }
        [SitecoreField(Fields.EmailAddress)]

        public virtual string EmailAddress { get; set; }
        [SitecoreField(Fields.Creator)]

        public virtual string Creator { get; set; }
        [SitecoreField(Fields.ProfileImage)]

        public virtual Image ProfileImage { get; set; }

        [SitecoreField(Fields.FullName)]
        public virtual string FullName { get; set; }

        [SitecoreField(Fields.Bio)]
        public virtual string Bio { get; set; }

        [SitecoreField(Fields.Location)]
        public virtual string Location { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        #endregion
        #region Fields
        public struct Fields
        {
            public const string Title = "Title";
            public const string EmailAddress = "Email Address";
            public const string Creator = "Creator";
            public const string ProfileImage = "Profile Image";
            public const string FullName = "Full Name";
            public const string Bio = "Bio";
            public const string Location = "Location";
        }
        #endregion

    }
}