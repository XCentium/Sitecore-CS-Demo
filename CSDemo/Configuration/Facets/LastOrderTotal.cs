using Sitecore.Analytics.Model.Framework;
using System;

namespace CSDemo.Configuration.Facets
{
    [Serializable]
    public class LastOrderTotal : Facet, ILastOrderTotal
    {
        public static readonly string _FACET_NAME = "LastOrderTotal";
        private const string _UPDATED_NAME = "LastUpdated";

        public LastOrderTotal()
        {
            EnsureAttribute<decimal>(_FACET_NAME);
            EnsureAttribute<DateTime>(_UPDATED_NAME);
        }

        public decimal Amount
        {
            get
            {
                return GetAttribute<decimal>(_FACET_NAME);
            }
            set
            {
                SetAttribute(_FACET_NAME, value);
            }
        }

        public DateTime Updated
        {
            get
            {
                return GetAttribute<DateTime>(_UPDATED_NAME);
            }
            set
            {
                SetAttribute(_UPDATED_NAME, value);
            }
        }
    }
}