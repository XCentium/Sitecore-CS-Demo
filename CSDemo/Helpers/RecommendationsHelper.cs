using CSDemo.Configuration;
using CSDemo.Models.Page;
using CSDemo.Models.Recommendations;

namespace CSDemo.Helpers
{
    public class RecommendationsHelper
    {
        public static Recommendations GetFrequentlyBoughtTogetherRecommendations(string itemId, int numberOfResults)
        {
            var recommendations = GetItemToItemRecommendations(itemId, numberOfResults, GetSiteRecommendationsApiFbtBuildId());
            recommendations.Type = RecommendationType.Fbt;

            return recommendations;
        }

        public static Recommendations GetItemRecommendations(string itemId, int numberOfResults)
        {
            var recommendations = GetItemToItemRecommendations(itemId, numberOfResults, GetSiteRecommendationsApiBuildId());
            recommendations.Type = RecommendationType.ItemToItem;

            return recommendations;
        }

        public static Recommendations GetUserRecommendations(string userId, int numberOfResults)
        {
            var recommendations = GetUserToItemRecommendations(userId, numberOfResults, GetSiteRecommendationsApiBuildId());
            recommendations.Type = RecommendationType.UserToItem;

            return recommendations;
        }

        private static Recommendations GetItemToItemRecommendations(string itemId, int numberOfResults, string buildId)
        {
            var recommendations = new Recommendations { RecommendationForItemId = itemId };

            var api = new Business.Services.RecommendationsApi(ConfigurationHelper.GetRecommendationsApiKey(),
                ConfigurationHelper.GetRecommendationsApiBaseUri());

            var items = api.GetRecommendations(GetSiteRecommendationsApiModelId(), buildId,
                itemId, numberOfResults);

            foreach (var recoSet in items.RecommendedItemSetInfo)
            {
                foreach (var item in recoSet.Items)
                {
                    recommendations.ItemsIds.Add(item.Id);
                }
            }

            return recommendations;
        }

        private static Recommendations GetUserToItemRecommendations(string userId, int numberOfResults, string buildId)
        {
            var recommendations = new Recommendations { RecommendationForUserId = userId };

            var api = new Business.Services.RecommendationsApi(ConfigurationHelper.GetRecommendationsApiKey(),
                ConfigurationHelper.GetRecommendationsApiBaseUri());

            var items = api.GetUserRecommendations(GetSiteRecommendationsApiModelId(), buildId,
                userId, numberOfResults);

            foreach (var recoSet in items.RecommendedItemSetInfo)
            {
                foreach (var item in recoSet.Items)
                {
                    recommendations.ItemsIds.Add(item.Id);
                }
            }

            return recommendations;
        }

        private static string GetSiteRecommendationsApiFbtBuildId()
        {
            var root = GetSiteRoot();

            return root.RecommendationApiFbtBuildId;
        }

        private static string GetSiteRecommendationsApiBuildId()
        {
            var root = GetSiteRoot();

            return root.RecommendationApiBuildId;
        }

        private static string GetSiteRecommendationsApiModelId()
        {
            var root = GetSiteRoot();

            return root.RecommendationApiModelId;
        }

        internal static Root GetSiteRoot()
        {
            // Fetch the start item from Site definition

            var rootItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.ContentStartPath);

            return rootItem == null ? null : GlassHelper.Cast<Root>(rootItem);
        }
    }
}