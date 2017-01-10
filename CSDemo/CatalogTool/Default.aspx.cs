using System;
using System.Text;
using Sitecore.Data.Items;

namespace CSDemo.CatalogTool
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCatlogAccordion();
        }

        private void LoadCatlogAccordion()
        {

            // Get all catalogs and list as accordion

            var catalogPath = "/sitecore/Commerce/Catalog Management";

            Item catalogManager = Sitecore.Context.Database.GetItem(catalogPath);

            if (catalogManager != null)
            {
                if (catalogManager.HasChildren)
                {
                    StringBuilder sb = new StringBuilder();


                }
            }

            throw new System.NotImplementedException();
        }
    }
}