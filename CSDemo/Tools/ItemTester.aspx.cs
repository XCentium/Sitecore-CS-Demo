using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSDemo.Helpers;
using CSDemo.Models.Product;
using Sitecore.Commerce.Connect.CommerceServer.Controls;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Shell.Applications.ContentEditor;

namespace CSDemo.Tools
{
    public partial class ItemTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var itemId = new ID("{833102C4-765F-4FA5-8DBD-A7F785D41659}"); //boots category
            var item = Sitecore.Context.Database.GetItem(itemId);

            string name;
            if (item != null)
            {
                var cat = GlassHelper.Cast<Category>(item);
                var x = cat.ChildProducts;

                ChildProductsListControl y = new ChildProductsListControl();
            }
        }
    }
}