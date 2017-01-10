using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CSDemo.CatalogTool
{
    public partial class Default : System.Web.UI.Page
    {
        public string CategoryExcludedFields =
            "CatalogName,ChildProducts,Runtime Search Facets,ChildCategories,Relationship List,DefinitionName,PrimaryParentCategory,Sort Fields,Items Per Page,Tools Search Facets,Tools Navigation Facets,ParentCategories,ToolsIcon";

        public string ProductExcludedFields =
             "CatalogName,Tools Navigation Facets,Relationship List,DefinitionName,PrimaryParentCategory,IntroductionDate,Tools Search Facets,Sort Fields,Items Per Page,ParentCategories,Variants,ToolsIcon,Runtime Search Facets";

        public string VariantsExcludedFields = "Tools Navigation Facets,Runtime Search Facets,Sort Fields,Items Per Page,Tools Search Facets,ToolsIcon";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadProductTypes();
                LoadCatalogRoot();

            }

        }


        private void LoadCatlogAccordion()
        {

            // Get all catalogs and list as accordion
            // get all items with templateName = Commerce Catalog Folder

            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
            var sitecoreItem = database.SelectSingleItem("/sitecore");
            if (sitecoreItem != null)
            {

            }

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

        private void LoadCatalogRoot()
        {
            // get all items with templateName = Commerce Catalog Folder

            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
            var query = string.Format("fast://*[@@templatename = 'Commerce Catalog Folder']");
            Sitecore.Data.Items.Item[] dItems =
                database.SelectItems(query);
            if (dItems != null && dItems.Any())
            {

                Dictionary<string, string> results = new Dictionary<string, string>();
                var firstPath = string.Empty;

                foreach (Item catalogRootItem in dItems)
                {
                    if (!catalogRootItem.Paths.Path.ToLower().Contains("_standard"))
                    {
                        results.Add(catalogRootItem.Paths.Path, catalogRootItem.Paths.Path);
                        if (firstPath == String.Empty)
                        {
                            firstPath = catalogRootItem.Paths.Path;
                        }
                    }
                }


                ddlCatalogRoot.DataSource = results;
                ddlCatalogRoot.DataTextField = "Value";
                ddlCatalogRoot.DataValueField = "Key";
                ddlCatalogRoot.DataBind();

                LoadCatalog();

            }


        }


        protected void ddlCatalogRoot_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCatalog();
        }

        private void LoadCatalog()
        {
            string thePath = ddlCatalogRoot.SelectedValue;
            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");

            var query = string.Format("fast://*[@@templatename = 'Commerce Catalog']");

            Response.Write("query = " + query + "<hr/>");

            List<Item> dItems = new List<Item>();

            if (!string.IsNullOrEmpty(thePath))
            {
                dItems = database.GetItem(thePath).GetChildren().ToList();
            }

            if (dItems != null && dItems.Any())
            {

                Dictionary<string, string> results = new Dictionary<string, string>();
                var firstPath = string.Empty;
                foreach (Item catalogItem in dItems)
                {
                    if (catalogItem.TemplateName == "Commerce Catalog")
                    {
                        results.Add(catalogItem.Name, catalogItem.Paths.Path);
                        if (firstPath == String.Empty)
                        {
                            firstPath = catalogItem.Paths.Path;
                        }
                    }
                }


                ddlCatalog.DataSource = results;
                ddlCatalog.DataValueField = "Value";
                ddlCatalog.DataTextField = "Key";
                ddlCatalog.DataBind();

                LoadCategories();


            }

            //lblCatalog.Text = ddlCatalog.SelectedValue;
        }


        protected void ddlCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            string thePath = ddlCatalog.SelectedValue;

            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");

            List<Item> dItems = new List<Item>();
            if (!string.IsNullOrEmpty(thePath))
            {
                dItems =
                    database.GetItem(thePath)
                        .Axes.GetDescendants()
                        .Where(x => x.TemplateName == "GeneralCategory")
                        .ToList();
            }

            if (dItems.Any())
            {

                Dictionary<string, string> results = new Dictionary<string, string>();
                var firstPath = string.Empty;
                results.Add("- Select a category -", "");
                foreach (Item catalogItem in dItems)
                {
                    if (catalogItem.TemplateName == "GeneralCategory")
                    {
                        results.Add(catalogItem.Paths.Path, catalogItem.Paths.Path);

                    }
                }


                ddlCategories.DataSource = results;
                ddlCategories.DataValueField = "Value";
                ddlCategories.DataTextField = "Key";
                ddlCategories.DataBind();

                LoadProducts();

            }

            lblCategory.Text = ddlCategories.SelectedValue;
        }

        private void LoadProducts()
        {
            var thePath = !string.IsNullOrEmpty(ddlCategories.SelectedValue) ? ddlCategories.SelectedValue : ddlCatalog.SelectedValue;
            var productTemplate = ddlProductTypes.SelectedValue;

            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");

            List<Item> dItems = new List<Item>();

            if (!String.IsNullOrEmpty(thePath))
            {

                if (productTemplate == "")
                {
                    dItems =
                        dItems =
                            database.GetItem(thePath)
                                .GetChildren()
                                .Where(x => x.TemplateName != "GeneralCategory")
                                .ToList();
                }
                else
                {
                    dItems =
                        dItems =
                            database.GetItem(thePath)
                                .GetChildren()
                                .Where(x => x.TemplateName == productTemplate)
                                .ToList();
                }
            }

            Dictionary<string, string> results = new Dictionary<string, string>();

            if (dItems != null && dItems.Any())
            {



                foreach (Item productItem in dItems)
                {
                    if (!productItem.Name.Contains("Variant"))
                    {
                        results.Add(String.Format("{0}|{1}", productItem.TemplateName, productItem.Name),
                            productItem.Paths.Path);
                    }
                }


            }

            ddlProducts.DataSource = results;
            ddlProducts.DataValueField = "Value";
            ddlProducts.DataTextField = "Key";
            ddlProducts.DataBind();


            LoadProductVariants();

            lblProduct.Text = ddlProducts.SelectedValue;
        }

        private void LoadProductTypes()
        {
            var thePath = "/sitecore/templates/Commerce/Catalog Generated";

            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
            var dItems = database.GetItem(thePath).GetChildren().ToArray();

            Dictionary<string, string> results = new Dictionary<string, string>();

            if (dItems != null && dItems.Any())
            {


                results.Add("- All -", "");
                foreach (Item productTypItem in dItems)
                {
                    if (productTypItem.Name != "GeneralCategory" && !productTypItem.Name.ToLower().Contains("variant"))
                    {
                        results.Add(productTypItem.Name, productTypItem.Name);

                    }
                }

            }

            ddlProductTypes.DataSource = results;
            ddlProductTypes.DataValueField = "Value";
            ddlProductTypes.DataTextField = "Key";
            ddlProductTypes.DataBind();

            LoadProducts();

        }

        private void LoadProductVariants()
        {
            Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
            Dictionary<string, string> results = new Dictionary<string, string>();
            var thePath = ddlProducts.SelectedValue;
            if (!string.IsNullOrEmpty(thePath))
            {
                var dItem = database.GetItem(thePath);

                var dItems = new List<Item>();
                if (dItem != null && dItem.HasChildren)
                {

                    dItems = dItem.GetChildren().ToList();

                }
                if (dItems.Any())
                {



                    foreach (Item productItem in dItems)
                    {

                        results.Add(productItem.Name, productItem.Paths.Path);

                    }

                }
            }
            ddlProductVariants.DataSource = results;
            ddlProductVariants.DataValueField = "Value";
            ddlProductVariants.DataTextField = "Key";
            ddlProductVariants.DataBind();


            lblVariants.Text = ddlProductVariants.SelectedValue;

        }

        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCategory.Text = ddlCategories.SelectedValue;
            LoadProducts();
        }

        protected void ddlProductTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
            lblProductTypeDownload.Text = ddlProductTypes.SelectedValue;
            if (ddlProductTypes.SelectedValue != "")
            {
                lnkDownloadCSVTemplateProductType.Enabled = true;
            }
            else
            {
                lnkDownloadCSVTemplateProductType.Enabled = false;
            }
        }

        protected void ddlProductVariants_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnGetItemProperties_Click(object sender, EventArgs e)
        {
            var id = txtId.Text.Trim();
            if (!string.IsNullOrEmpty(id) && id.Contains("{"))
            {
                Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
                var dItem = database.GetItem(new ID(id));
                if (dItem != null)
                {
                    if (dItem.TemplateName.ToLower() == "generalcategory")
                    {
                        GetCategoryPropeties(dItem.Paths.Path);
                    }
                    if (dItem.TemplateName.ToLower() != "generalcategory" && !dItem.TemplateName.ToLower().Contains("variant"))
                    {
                        GetProductsPropeties(dItem.Paths.Path);
                    }
                    if (dItem.TemplateName.ToLower() != "generalcategory" && dItem.TemplateName.ToLower().Contains("variant"))
                    {
                        GetProductVariantsPropeties(dItem.Paths.Path);
                    }
                }
            }
        }
        private void GetCategoryPropeties(string p)
        {
            GetItemsField(p, "category");
        }



        private void GetProductsPropeties(string p)
        {
            GetItemsField(p, "products");
        }


        private void GetProductVariantsPropeties(string p)
        {
            GetItemsField(p, "variant");
        }

        private string GetItemsField(string path, string thetype)
        {
            Response.Write("<h1>" + thetype + "</h1>");

            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("master");
            Item currentItem = database.GetItem(path);


            List<string> fieldNames = new List<string>();
            currentItem.Fields.ReadAll();
            Sitecore.Collections.FieldCollection fieldCollection = currentItem.Fields;

            //Response.Write("<h2>Standard Field</h2><hr/>");
            //Response.Write("<table align='left' border='0' cellspacing='0' cellpadding='0' width='100%'><tr align='left' valign='top'><th>Field</th><th>Value</th>");
            //Response.Write("<tr><td>ID:</td><td>" + currentItem.ID + "</td></tr>");
            //Response.Write("<tr><td>Name:</td><td>" + currentItem.Name + "</td></tr>");
            //Response.Write("<tr><td>Template Name:</td><td>" + currentItem.TemplateName + "</td></tr>");
            //Response.Write("<tr><td>Template Id:</td><td>" + currentItem.TemplateID.ToString() + "</td></tr>");
            //Response.Write("<tr><td>Path:</td><td>" + currentItem.Paths.Path + "</td></tr>");
            //foreach (Field field in fieldCollection)
            //{
            //    //Use the following check if you do not want 
            //    //the Sitecore Standard Fields 

            //    if (field.Name.StartsWith("__"))
            //    {
            //        String fname = field.Name.ToString();
            //        // Response.Write(fname + " : value = " + currentItem[fname].ToString() + "<hr />");
            //        Response.Write("</tr><tr align='left' valign='top'>	<td>" + fname + "</td><td>" + currentItem[fname].ToString() + "</td></tr>");
            //    }

            //}
            //Response.Write("</table>");


            //Response.Write("<h2>Other Field</h2><hr/>");
            //Response.Write("<table align='left' border='0' cellspacing='0' cellpadding='0' width='100%'><tr align='left' valign='top'><th>Field</th><th>Value</th>");

            //foreach (Field field in fieldCollection)
            //{
            //    //Use the following check if you do not want 
            //    //the Sitecore Standard Fields 
            //    if (!field.Name.StartsWith("__"))
            //    {
            //        String fname = field.Name.ToString();
            //        Response.Write("</tr><tr align='left' valign='top'>	<td>" + fname + "</td><td>" + currentItem[fname].ToString() + "</td></tr>");
            //    }

            //}
            //Response.Write("</table>");

            var fieldsList = new List<string>();
            fieldsList.Add("ID");
            fieldsList.Add("Name");
            fieldsList.Add("Template Name");
            fieldsList.Add("ParentPath");
            fieldsList.Add("__Display name");

            var fieldsListExclude = new List<string>();

            switch (thetype)
            {
                case "category":

                    fieldsListExclude = CategoryExcludedFields.Split(',').ToList();

                    break;

                case "products":

                    fieldsListExclude = ProductExcludedFields.Split(',').ToList();

                    break;

                case "variant":

                    fieldsListExclude = VariantsExcludedFields.Split(',').ToList();


                    break;



                default:
                    break;
            }

            foreach (Field field in fieldCollection)
            {
                if (!field.Name.StartsWith("__") && !fieldsListExclude.Contains(field.Name))
                {
                    fieldsList.Add(field.Name);
                }
            }

            //Response.Write("<hr>" + String.Join(",", fieldsListExclude));

            Response.Write("<hr>" + String.Join(",", fieldsList));

            return String.Join(",", fieldsList);
        }

        protected void lnkDownloadCSVTemplateCategory_Click(object sender, EventArgs e)
        {
            // get 1 category item, if non exist, create one

            if (ddlCategories.Items.Count > 1)
            {

                string csv = GetItemsField(ddlCategories.Items[1].Value, "category");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=CategoryImportTemplate.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }




        }

        protected void lnkDownloadCSVTemplateProductType_Click(object sender, EventArgs e)
        {

        }

        protected void lnkDownloadCSVTemplateProductVariantType_Click(object sender, EventArgs e)
        {

        }

        protected void lnkDownloadAllCategories_Click(object sender, EventArgs e)
        {
            var catPath = string.Empty;
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("master");
            if (ddlCategories.Items.Count > 1)
            {
                catPath = ddlCategories.Items[1].Value;

                string csvTopRow = GetItemsField(catPath, "category");
                // string csv = ;

                var csv = new StringBuilder();

                csv.AppendLine(csvTopRow);

                foreach (ListItem theItemPath in ddlCategories.Items)
                {
                    Item dItem = database.GetItem(theItemPath.Value);
                    if (dItem != null)
                    {
                        //Add new line.

                        csv.AppendLine(GetItemCsvRow(dItem, csvTopRow));

                    }

                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=CategoryExport" + DateTime.UtcNow.Ticks.ToString() + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv.ToString());
                Response.Flush();
                Response.End();
            }




        }

        private string GetItemCsvRow(Item dItem, string csvTopRow)
        {
            StringBuilder csvRow = new StringBuilder();



            var csvRowList = new List<string>();


            csvRow.Append(dItem.ID.ToString());
            csvRow.Append(",");
            csvRow.Append(dItem.Name);
            csvRow.Append(",");
            csvRow.Append(dItem.TemplateName);
            csvRow.Append(",");
            csvRow.Append(dItem.Paths.ParentPath);
            csvRow.Append(",");
            csvRow.Append(dItem.DisplayName);


            //Response.Write(csvTopRow);
            //Response.Write("<hr/>");


            var fieldList = csvTopRow.Replace("ID,Name,Template Name,ParentPath,__Display name,", "").Split(',').ToList();
            if (fieldList.Any())
            {
                foreach (var fieldKey in fieldList)
                {
                    csvRow.Append(",");
                    var s = CleanCsvString(dItem[fieldKey].ToString());
                    if (s.Contains(","))
                    {
                        s = "=" + s;
                        Response.Write(fieldKey + " ||| " + s + "<hr/>");
                    }
                    csvRow.Append(s);

                }

            }
            //Response.Write("<br>-------------------------------------------------------------------------------------<br>");
            return csvRow.ToString();
        }

        protected string CleanCsvString(string input)
        {
            string output = "\"" + input.Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", "") + "\"";
            return output;
        }


    }
}