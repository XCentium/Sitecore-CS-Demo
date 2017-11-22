using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
	public class BlackListedProductsForInmateModel
	{
		public List<InmateCatalog> Blacklist;

		public BlackListedProductsForInmateModel()
		{
			Blacklist = new List<InmateCatalog>();
		}


		public void PopulateSampleData()
		{
			InmateCatalog c = new InmateCatalog();

			c.InmateId = "123452";
			c.Products = new List<string>() { "Snickers-1-86-oz-Bar" };
			Blacklist.Add(c);

			c = new InmateCatalog();

			c.InmateId = "123452";
			c.Products = new List<string>() { "3-Musketeers-1-92-oz-Bar" };
			Blacklist.Add(c);

			c = new InmateCatalog();

			c.InmateId = "123453";
			c.Products = new List<string>() { "Doritos-8-oz-Nacho-Cheese-Chips" };
			Blacklist.Add(c);


			var str = Newtonsoft.Json.JsonConvert.SerializeObject(Blacklist);			
		}
	}

	public class InmateCatalog
	{
		public InmateCatalog()
		{
			Products = new List<string>();
		}
		public string InmateId;
		public List<string> Products; 
	}

    public class InmateProductRestrictions
    {
        public List<string> Restrictions;

        public InmateProductRestrictions()
        {
            Restrictions = new List<string>();
        }

        public void PopulateSampleData(string inmateId)
        {
            switch (inmateId)
            {
                case "123451":
                    Restrictions.Add("male");
                    break;
                case "123452":
                    Restrictions.Add("female");
                    break;
                case "123453":
                    Restrictions.Add("kosher");
                    Restrictions.Add("glutenfree");
                    break;
                default:
                    break;
            }
        }
    }
}
