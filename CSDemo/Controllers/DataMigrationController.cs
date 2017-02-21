using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Configuration;

namespace CSDemo.Controllers
{
    public class DataMigrationController : Controller, IController
    {
        public String ProductRootPath = "/sitecore/Commerce/Catalog Management/Catalogs/Keefe";
        public Database DB
        {
            get
            {
                return Factory.GetDatabase("master");
            }
        }
        public TemplateID CATEGORY_TEMPLATE_ID
        {
            get {
                return GetTemplateId("/sitecore/templates/Commerce/Catalog Generated/GeneralCategory");
            }
        }
        
        public TemplateID PRODUCT_TEMPLATE_ID
        {
            get
            {
                return GetTemplateId("/sitecore/templates/Commerce/Catalog Generated/Backpack");
            }
        }

        public TemplateID GetTemplateId(string path)
        {
            Item itemTemplate = DB.GetItem(path);

            return new TemplateID(itemTemplate.ID);
        }

        public DataMigrationController():base()
        {
            ;
        }

        // GET: DataMigration
        public ActionResult Index(bool rebuild = false)
        {
            var database = Factory.GetDatabase("master");

            using (var csv = new CsvReader(new StreamReader(Server.MapPath(@"~/DataImport/VendorData.csv"))))
            {
                csv.ReadHeader();
                string[] headers = csv.FieldHeaders;

                Item rootPath = database.GetItem(ProductRootPath);

                if (rebuild)
                {
                    using (new SecurityDisabler())
                    {
                        rootPath.DeleteChildren();
                    }
                }

                while (csv.Read())
                {
                    ImportRow(headers, csv, database);
                }
            }

            //header

            return View();
        }

        private void ImportRow(string[] headers, CsvReader csv, Database database)
        {
            using (new SecurityDisabler())
            {  
                Item productItem = GetItem(database, csv);

                if (productItem != null)
                {
                    using (new EditContext(productItem))
                    {
                        int i = 0;
                        foreach (string currentColumn in headers)
                        {
                            string columnName = GetColumnName(currentColumn);
                            productItem[columnName] = csv[i];
                            i++;

                        }
                    }
                }
            }
        }

        private string GetColumnName(string currentColumn)
        {
            switch (currentColumn)
            {
                case "price":
                    return "ListPrice";
                case "name":
                    return "Title";
            }

            return currentColumn;
        }

        private Item GetItem(Database database, CsvReader csv)
        {
            string menu = csv["Menu"];
            string name = csv["name"];
            string parentCategory = csv["parentCategory"];
            string category = csv["category"];

            menu = CleanName(menu);
            name = CleanName(name);

            if (!String.IsNullOrWhiteSpace(parentCategory))
            {
                parentCategory = CleanName(menu + " " + parentCategory);
            }

            if (!String.IsNullOrWhiteSpace(category))
            {
                category = CleanName(menu + " " + category);
            }
            
            Item rootPath = database.GetItem(ProductRootPath);
            Item currentItem = rootPath;
            
            if (String.IsNullOrWhiteSpace(menu) || menu == "Menu")
                return null;

            currentItem = GetOrCreateItem(menu, currentItem, CATEGORY_TEMPLATE_ID);

            if (!String.IsNullOrWhiteSpace(parentCategory))
            {
                currentItem = GetOrCreateItem(parentCategory, currentItem, CATEGORY_TEMPLATE_ID);
            }

            //Skip if the parent category is same as child category or the category is null
            if (!String.IsNullOrWhiteSpace(category) && parentCategory != category)
            {
                currentItem = GetOrCreateItem(category, currentItem, CATEGORY_TEMPLATE_ID);
            }

            if(String.IsNullOrWhiteSpace(name))
            {
                Log.Warn("Failed to lookup name", String.Empty);
                return null;
            }
            else if(currentItem == null)
            {
                Log.Warn("Failed to lookup category: " + parentCategory + ", " + category, String.Empty);
                return null;
            }
            else
            {
                return GetOrCreateItem(name, currentItem, PRODUCT_TEMPLATE_ID);
            }
        }

        private static Item GetOrCreateItem(string itemName, Item parentItem, TemplateID templateId)
        {
            Item matchItem = parentItem.Children[itemName];

            if (matchItem != null)
                return matchItem;

            try
            {
                return parentItem.Add(itemName, templateId);
            }
            catch(Exception e)
            {
                Log.Fatal("Failed to create item: " + itemName, e, String.Empty);
                return null;
                //throw new Exception("Failed to create item: " + itemName, e);
            }
        }

        private string CleanName(string name)
        {
            string cleanName = name.Replace(".", " ");
            cleanName = cleanName.Replace("/", " ");
            cleanName = cleanName.Replace("?", " ");
            cleanName = cleanName.Replace("'", " ");
            cleanName = cleanName.Replace("(", " ");
            cleanName = cleanName.Replace(")", " ");
            cleanName = cleanName.Replace("&", " ");
            cleanName = cleanName.Replace("%", " ");
            cleanName = cleanName.Replace("\"", " ");
            cleanName = cleanName.Replace("!", " ");
            cleanName = cleanName.Replace("#", " ");
            cleanName = cleanName.Replace("*", " ");
            cleanName = cleanName.Replace(",", " ");
            cleanName = cleanName.Replace("\\", " ");

            cleanName = cleanName.Trim();

            while (cleanName.Contains("  "))
            {
                cleanName = cleanName.Replace("  ", " ");
            }

            cleanName = cleanName.Replace(" ", "-");

            return cleanName;
        }
    }
}