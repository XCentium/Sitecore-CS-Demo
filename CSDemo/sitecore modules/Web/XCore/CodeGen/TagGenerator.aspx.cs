using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.StringExtensions;

namespace CSDemo.sitecore_modules.Web.XCore.CodeGen
{
    public struct TagConstants
    {
        // /sitecore/templates/CSDemo/Components/Tagging/Tag
        public const string TagTemplateId = "{3C937B2D-EA5E-4CB0-8FBD-1799BFA30475}";

        // /sitecore/Commerce/Catalog Management/Catalogs/Adventure Works Catalog
        public const string StartCatalogId = "{2C1225CC-24C0-496B-AEB7-7BEF84C0252A}";

        // /sitecore/content/CSDemo/Components/Tags
        public const string TagContainerId = "{E95DAA23-11D6-4C06-BEED-53823DAA948D}";

        public const string BaseWildcardMatch = "@@templatename = 'GeneralCategory'";
    }

    public partial class TagGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TagTemplateId.Text = TagConstants.TagTemplateId;
            TagContainerId.Text = TagConstants.TagContainerId;
            StartCatalogId.Text = TagConstants.StartCatalogId;
            BaseWildcardMatch.Text = TagConstants.BaseWildcardMatch;
        }

        public void GenerateTagsFromRootPath(string baseWildcardMatch, string tagContainerId, string startCatalogId, string tagTemplateId, List<string> excludedTemplateNamesContains)
        {
            Database db = Sitecore.Configuration.Factory.GetDatabase("master");
            var targetCatalogRoot = db.GetItem(startCatalogId);
            string startPath = targetCatalogRoot.Paths.Path;
            string startWildCardMatch = "";
            if (!string.IsNullOrEmpty(startWildCardMatch))
            {
                startWildCardMatch = baseWildcardMatch;
            }
            string andWildcardMatches = "";
            bool andFlag = startWildCardMatch.IsNullOrEmpty();
            if (excludedTemplateNamesContains.Any())
            {
                for(var i = 0;i<excludedTemplateNamesContains.Count;i++)
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
            var startPathSplitter = startPath.Split('/');
            List<string> usablePaths = new List<string>();
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
            var query = $"{startPath}//*[{startWildCardMatch}{andWildcardMatches}]";
            var items = db.SelectItems(query);
            if (items == null || !items.Any()) return;
            // Create a list of tags and filter item display name noise
            List<string> tags = items.Select(item => item.DisplayName).Where(t => !t.Contains(".") && !t.Contains("(") && !t.Contains("-") && !t.Contains("'")).ToList();
            var tagsToCreate = tags.Distinct().ToList();
            foreach (var tag in tagsToCreate)
            {
                if(!DryRunOnly.Checked)
                    CreateTagItem(tag);
                Message.Text += $"Created Tag <b>'{tag}'</b><br />";
            }
        }

        public void CreateTagItem(string tag)
        {
            using (new SecurityDisabler())
            {
                Database db = Sitecore.Configuration.Factory.GetDatabase("master");
                Item parentItem = db.GetItem(TagConstants.TagContainerId);
                TemplateItem template = db.GetTemplate(TagConstants.TagTemplateId);
                if (parentItem.Children.Any(t => t.Name == tag)) return;
                var newTagItem = parentItem.Add(tag, template);
            }
        }

        protected void ButtonGenerate_Click(object sender, EventArgs e)
        {
            Message.Text = "";
            var tagTemplateId = TagConstants.TagTemplateId; // default
            var startCatalogId = TagConstants.StartCatalogId;  // default
            var tagContainerId = TagConstants.TagContainerId; // default
            var baseWildcardMatch = "";
            if (TagTemplateId.Text != string.Empty && Sitecore.Data.ID.IsID(TagTemplateId.Text))
                tagTemplateId = TagTemplateId.Text;
            if (StartCatalogId.Text != string.Empty && Sitecore.Data.ID.IsID(StartCatalogId.Text))
                startCatalogId = StartCatalogId.Text;
            if (TagContainerId.Text != string.Empty && Sitecore.Data.ID.IsID(TagContainerId.Text))
                tagContainerId = TagContainerId.Text;
            if (BaseWildcardMatch.Text != string.Empty && Sitecore.Data.ID.IsID(BaseWildcardMatch.Text))
                tagContainerId = BaseWildcardMatch.Text;
            // var excludedTemplateIds = ExcludedTemplateIds.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var excludedTemplateNamesContains = new List<string>(ExcludedTemplateNamesContains.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            GenerateTagsFromRootPath(baseWildcardMatch, tagTemplateId, startCatalogId, tagContainerId, excludedTemplateNamesContains);
        }
    }
}