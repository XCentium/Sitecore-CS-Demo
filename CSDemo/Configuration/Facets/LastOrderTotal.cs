using Sitecore.Analytics.Model.Framework;
using System;

namespace CSDemo.Configuration.Facets
{
    [Serializable]
    public class LastOrderTotal : Facet, ILastOrderTotal
    {
        public static readonly string FacetName = "LastOrderTotal";
        private const string UpdatedName = "LastUpdated";

        public LastOrderTotal()
        {
            EnsureAttribute<decimal>(FacetName);
            EnsureAttribute<DateTime>(UpdatedName);
        }

        public decimal Amount
        {
            get
            {
                return GetAttribute<decimal>(FacetName);
            }
            set
            {
                SetAttribute(FacetName, value);
            }
        }

        public DateTime Updated
        {
            get
            {
                return GetAttribute<DateTime>(UpdatedName);
            }
            set
            {
                SetAttribute(UpdatedName, value);
            }
        }
    }
}