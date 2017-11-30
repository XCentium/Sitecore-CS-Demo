using KeefePOC.Models.Enumerations;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
    public class Program
    {
        public Program() { }

        public Program(Item programItem)
        {
            Id = programItem.ID.Guid.ToString();
            Name = programItem["Name"];
            ProgramHomePage = programItem["Program Home Page"];
            ProgramType = (ProgramType)Enum.Parse(typeof(ProgramType), programItem["Program Type"]);
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string State { get; set; }

        public ProgramType ProgramType { get; set; }

        public Guid CatalogId { get; set; }

        public double WeightLimit { get; set; }
        public List<object> PurchaserWhitelist { get; set; }

        public List<Facility> Facilities { get; set; }

        public string ExternalId { get; set; }
        public bool IsActive { get; set; }

        public string ProgramHomePage { get; set; }

        public decimal QuarterlyOrderWeightLimit { get; set; }
        public decimal QuarterlyOrderPriceLimit { get; set; }
    }
}
