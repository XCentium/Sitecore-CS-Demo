using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.SecurityModel;
using Sitecore.StringExtensions;

namespace CSDemo.sitecore_modules.Web.XCore.CodeGen
{
    public struct TagConstants
    {
        // /sitecore/templates/CSDemo/Components/Tagging/Tag
        public const string TagTemplateId = "{3C937B2D-EA5E-4CB0-8FBD-1799BFA30475}";
        // 	/sitecore/templates/Commerce/Catalog/Commerce Catalog Folder
        public const string CommerceCatalogFolderTemplateId = "{334E2B54-F913-411D-B159-A7B16D65242C}";
        // 	/sitecore/templates/Commerce/Catalog/Commerce Catalog
        public const string CommerceCatalogTemplateId = "{93AF861A-B6F4-45BE-887D-D93D4B95B39D}";
        // /sitecore/Commerce/Catalog Management/Catalogs/Adventure Works Catalog
        public const string StartCatalogId = "{2C1225CC-24C0-496B-AEB7-7BEF84C0252A}";
        // /sitecore/Commerce/Catalog Management/Catalogs
        public const string CatalogsContainerId = "{08BFB57F-DAC9-4B11-90C5-5D19647899EF}";
        // /sitecore/content/CSDemo/Components/Tags
        public const string TagContainerId = "{E95DAA23-11D6-4C06-BEED-53823DAA948D}";
        public const string BaseWildcardMatch = "@@templatename = 'GeneralCategory'";
        public const string BaseCatalogWildcardMatch = "@@templatename = 'Commerce Catalog'";
        // Error messages
        public const string DbWasNull = "Database was null.";
        public const string CatalogsContainerWasNull = "Database was null.";
        public const string NoCatalogsFound = "No Catalogs Were Found.";
    }

    public partial class TagGenerator : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TagTemplateId.Text = TagConstants.TagTemplateId;
            TagContainerId.Text = TagConstants.TagContainerId;
            BaseWildcardMatch.Text = TagConstants.BaseWildcardMatch;
            var db = Factory.GetDatabase("master");
            var catalogsContainer = db.GetItem(TagConstants.CatalogsContainerId);
            if (catalogsContainer == null)
            {
                Message.Text = TagConstants.CatalogsContainerWasNull;
                return;
            }
            var startPath = FixStartPath(catalogsContainer.Paths.Path);
            var query = $"{startPath}//*[{TagConstants.BaseCatalogWildcardMatch}]";
            var catalogs = db.SelectItems(query);
            if (catalogs == null || !catalogs.ToList().Any())
            {
                Message.Text = TagConstants.NoCatalogsFound;
                return;
            }
            foreach (var catalog in catalogs)
            {
                CatalogsDropDown.Items.Add(new ListItem(catalog.DisplayName, catalog.ID.ToString()));
            }
        }

        public void GenerateTagsFromRootPath(string baseWildcardMatch, string tagContainerId, string startCatalogId,
            string tagTemplateId, List<string> excludedTemplateNamesContains)
        {
            var db = Factory.GetDatabase("master");
            var targetCatalogRoot = db.GetItem(startCatalogId);
            var startWildCardMatch = "";
            if (!string.IsNullOrEmpty(startWildCardMatch))
            {
                startWildCardMatch = baseWildcardMatch;
            }
            var andWildcardMatches = "";
            var andFlag = startWildCardMatch.IsNullOrEmpty();
            if (excludedTemplateNamesContains.Any())
            {
                for (var i = 0; i < excludedTemplateNamesContains.Count; i++)
                {
                    if (i == 0 && andFlag)
                    {
                        andWildcardMatches = $"@@templatename != '{excludedTemplateNamesContains[i]}'";
                    }
                    else
                    {
                        andWildcardMatches = $"and @@templatename != '{excludedTemplateNamesContains[i]}'";
                    }
                }
            }
            // Add '#' around paths with spaces
            var startPath = FixStartPath(targetCatalogRoot.Paths.Path);
            var query = $"{startPath}//*[{startWildCardMatch}{andWildcardMatches}]";
            var items = db.SelectItems(query);
            if (items == null || !items.Any()) return;
            // Create a list of tags and filter item display name noise
            var tags =
                items.Select(item => item.DisplayName)
                    .Where(t => !t.Contains(".") && !t.Contains("(") && !t.Contains("-") && !t.Contains("'"))
                    .ToList();
            var tagsToCreate = tags.Distinct().ToList();
            foreach (var tag in tagsToCreate)
            {
                if (!DryRunOnly.Checked)
                    CreateTagItem(tag);
                Message.Text += $"Created Tag <b>'{tag}'</b><br />";
            }
        }

        private static string FixStartPath(string startPath)
        {
            var startPathSplitter = startPath.Split('/');
            var usablePaths = new List<string>();
            foreach (var path in startPathSplitter)
            {
                if (path.Contains(" "))
                {
                    usablePaths.Add($"#{path}#");
                }
                else
                {
                    usablePaths.Add(path);
                }
            }
            startPath = StringUtil.EnsurePrefix('/', string.Join("/", usablePaths));
            return startPath;
        }

        public void CreateTagItem(string tag)
        {
            using (new SecurityDisabler())
            {
                var db = Factory.GetDatabase("master");
                var parentItem = db.GetItem(TagConstants.TagContainerId);
                var template = db.GetTemplate(TagConstants.TagTemplateId);
                if (parentItem.Children.Any(t => t.Name == tag)) return;
                var newTagItem = parentItem.Add(tag, template);
            }
        }

        protected void ButtonGenerate_Click(object sender, EventArgs e)
        {
            Message.Text = "";
            var tagTemplateId = TagConstants.TagTemplateId; // default
            var startCatalogId = TagConstants.StartCatalogId; // default
            var tagContainerId = TagConstants.TagContainerId; // default
            var baseWildcardMatch = "";
            if (TagTemplateId.Text != string.Empty && Sitecore.Data.ID.IsID(TagTemplateId.Text))
                tagTemplateId = TagTemplateId.Text;
            if (CatalogsDropDown.Items.Count <= 0 || CatalogsDropDown.SelectedValue.IsEmptyOrNull())
            {
                Message.Text = TagConstants.NoCatalogsFound;
                return;
            }
            startCatalogId = CatalogsDropDown.SelectedValue;
            if (TagContainerId.Text != string.Empty && Sitecore.Data.ID.IsID(TagContainerId.Text))
                tagContainerId = TagContainerId.Text;
            if (BaseWildcardMatch.Text != string.Empty && Sitecore.Data.ID.IsID(BaseWildcardMatch.Text))
                tagContainerId = BaseWildcardMatch.Text;
            var excludedTemplateNamesContains =
                new List<string>(ExcludedTemplateNamesContains.Text.Split(new[] {"\r\n"},
                    StringSplitOptions.RemoveEmptyEntries));
            GenerateTagsFromRootPath(baseWildcardMatch, tagTemplateId, startCatalogId, tagContainerId,
                excludedTemplateNamesContains);
        }
    }
}