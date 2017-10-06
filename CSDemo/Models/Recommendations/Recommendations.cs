using System.Collections.Generic;

namespace CSDemo.Models.Recommendations
{
    public class Recommendations
    {
        public RecommendationType Type { get; set; }
        public string RecommendationForItemId { get; set; }
        public List<string> ItemsIds { get; set; }
        public string RecommendationForUserId { get; set; }

        public Recommendations()
        {
            ItemsIds = new List<string>();
        }
    }

    public enum RecommendationType
    {
        ItemToItem = 0,
        UserToItem,
        Fbt
    }
}