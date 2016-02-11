<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CodeGenerationRoot.aspx.cs" Inherits="AssetMark.sitecore_modules.Web.XCore.CodeGen.CodeGenerationRoot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            Namespace (eg. XCore.Models)<br />
            <asp:TextBox ID="tbNamespace" runat="server" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="frvNamespace" runat="server" ControlToValidate="tbNamespace"
                ErrorMessage="Please enter a name space"></asp:RequiredFieldValidator>
            <br />
            <br />
            Root in Template<br />
            <asp:TextBox ID="tbRootTemplateID" runat="server" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="frvRootTemplateID" runat="server" ControlToValidate="tbRootTemplateID"
                ErrorMessage="Please enter a root template id"></asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:CheckBox ID="cbIncludeBaseField" runat="server" Checked="false" Text="Include Base Fields" />
            <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click" />
            <br />
            <asp:Literal ID="litError" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
