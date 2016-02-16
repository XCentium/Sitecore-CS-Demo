using System;
using System.Linq;
using XCore.CodeGenerator;

namespace AssetMark.sitecore_modules.Web.XCore.CodeGen
{
    public partial class CodeGenerationRoot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            Sitecore.Data.ID templateRootID = Sitecore.Data.ID.Null;
            if (Sitecore.Data.ID.TryParse(tbRootTemplateID.Text, out templateRootID) && !string.IsNullOrEmpty(tbNamespace.Text))
            {
                var rootItem = Sitecore.Context.Database.GetItem(templateRootID);
                if (rootItem != null)
                {
                    var templates = rootItem.Axes.GetDescendants().Where(t => t.TemplateName == "Template");
                    foreach (var template in templates)
                    {
                        Manager.Generate(template.ID, tbNamespace.Text, cbIncludeBaseField.Checked);
                    }
                }
                
            }
            else
                litError.Text = "Something went wrong";
        }
    }
}