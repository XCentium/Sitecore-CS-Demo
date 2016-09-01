using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Configuration;
using Sitecore.Security.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CSDemo.Models.Account
{
    [SitecoreType(AutoMap =true)]
    public class UserStatus
    {
        [SitecoreInfo(SitecoreInfoType.Name)]
        public string Name { get; set; }

        [SitecoreField("Spending Amount")]
        public decimal Amount { get; set; }

        public Image Badge { get; set; }

        #region Methods

        public static UserStatus GetStatus(decimal amount)
        {
            var statuses = GetAllStatuses();
            if (!statuses.Any()) return null;
            var userStatus = new UserStatus { Amount = 0, Name = string.Empty };
            foreach (var status in statuses)
            {
                if (amount > status.Amount && status.Amount > userStatus.Amount)
                    userStatus = status;
            }

            return userStatus;
        }

        public static UserStatus GetStatus(User user)
        {
            if (!user.IsAuthenticated) return null;
            var statuses = GetAllStatuses();
            if (!statuses.Any()) return null;
            var status = statuses.FirstOrDefault(s => s.Name == user.Profile.Comment);

            return status;
        }

        public static UserStatus GetNextStatus(decimal amount)
        {
            var user = Sitecore.Context.User;
            if (!user.IsAuthenticated) return null;
            var currentStatus = GetStatus(user);
            var statuses = GetAllStatuses();
            var nextStatus = new UserStatus { Amount = 0, Name = string.Empty };
            foreach (var status in statuses)
            {
                if (status.Amount <= amount || (currentStatus!=null && status.Amount<= currentStatus.Amount)) continue;
                if (nextStatus.Amount == 0)
                {
                    nextStatus = status;
                    continue;
                }else
                {
                    if (status.Amount >= nextStatus.Amount && (currentStatus==null ||  status.Amount>currentStatus.Amount)) continue;
                    nextStatus = status;
                }
            }

            return nextStatus;
        }

        public static IEnumerable<UserStatus> GetAllStatuses()
        {
            var statusFolderItem = Factory.GetDatabase("master").GetItem(Settings.GetSetting("CSDemo.StatusParentItemId", "{206EFDD8-A567-4F13-9FD7-B661AFDFC7F4}"));
            if (statusFolderItem == null) return null;

            var statusItems = statusFolderItem.Children;
            if (!statusItems.Any()) return null;

            var statuses = statusItems.Select(i => (new SitecoreContext()).Cast<UserStatus>(i));
            return statuses;
        }

        #endregion
    }
}