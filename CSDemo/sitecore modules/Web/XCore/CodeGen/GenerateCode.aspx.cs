using System;

namespace AssetMark.sitecore_modules.Web.XCore.CodeGen
{
    public partial class GenerateCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            Sitecore.Data.ID templateID = Sitecore.Data.ID.Null;
            if (Sitecore.Data.ID.TryParse(tbTemplateID.Text, out templateID) && !string.IsNullOrEmpty(tbNamespace.Text))
                global::XCore.CodeGenerator.Manager.Generate(templateID, tbNamespace.Text, cbIncludeBaseField.Checked);
            else
                litError.Text = "Something went wrong ";
        }
    }
}