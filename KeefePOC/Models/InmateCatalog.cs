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


			c.InmateId = "123451";
			c.Products = new List<string>() { "10003" };
			Blacklist.Add(c);

			c.InmateId = "123452";
			c.Products = new List<string>() { "10001" };
			Blacklist.Add(c);

			c = new InmateCatalog();

			c.InmateId = "123455";
			c.Products = new List<string>() { "10002" };
			Blacklist.Add(c);

			c = new InmateCatalog();

			c.InmateId = "123453";
			c.Products = new List<string>() { "10004" };
			Blacklist.Add(c);

			c = new InmateCatalog();

			c.InmateId = "123456";
			c.Products = new List<string>() { "10004" };
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
