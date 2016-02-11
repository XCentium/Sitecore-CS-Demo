<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateCode.aspx.cs" Inherits="AssetMark.sitecore_modules.Web.XCore.CodeGen.GenerateCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
        <div>
            <br />
            <br />
            Namespace (eg. Nerium.Models)<br />
            <asp:TextBox ID="tbNamespace" runat="server" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="frvNamespace" runat="server" ControlToValidate="tbNamespace"
                ErrorMessage="Please enter a name space"></asp:RequiredFieldValidator>
            <br />
            <br />
            Template ID<br />
            <asp:TextBox ID="tbTemplateID" runat="server" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="frvTemplateID" runat="server" ControlToValidate="tbTemplateID"
                ErrorMessage="Please enter a template id"></asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:CheckBox ID="cbIncludeBaseField" runat="server" Checked="false" Text="Include Base Fields (Leave it blank)" />
            <br />
            <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click" />
            <br />
            <asp:Literal ID="litError" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
