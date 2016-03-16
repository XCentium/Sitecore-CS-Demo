<%@ page language="C#" autoeventwireup="true" codebehind="TagGenerator.aspx.cs" inherits="CSDemo.sitecore_modules.Web.XCore.CodeGen.TagGenerator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 5px">
            Tag Template Identifier
        <asp:textbox id="TagTemplateId" runat="server" width="400px"></asp:textbox>
        </div>
        <div style="padding: 5px">
            Start Catalog Identifier
        <asp:textbox id="StartCatalogId" runat="server" width="400px"></asp:textbox>
        </div>
        <div style="padding: 5px">
            Tag Container Identifier
        <asp:textbox id="TagContainerId" runat="server" width="400px"></asp:textbox>
        </div>
        <div style="padding: 5px">
            Tag Container Identifier
        <asp:textbox id="BaseWildcardMatch" runat="server" width="400px"></asp:textbox>
        </div>
       <%-- <div style="padding: 5px">
            Exclude Templates (By Id)<br/>
            <asp:TextBox ID="ExcludedTemplateIds" runat="server" TextMode="MultiLine" Rows="10" width="400px"></asp:TextBox>
        </div>--%>
        <div style="padding: 5px">
            Exclude Templates (By Contains Text)<br />
            <asp:TextBox ID="ExcludedTemplateNamesContains" runat="server" TextMode="MultiLine" Rows="10" width="400px"></asp:TextBox>
        </div>
        <div style="padding: 5px">
            Generate Tags <asp:button runat="server" id="ButtonGenerate" text="Generate" OnClick="ButtonGenerate_Click" /><br /><br />
            Dry Run (Message Output Only? <asp:CheckBox runat="server" ID="DryRunOnly"/>
        </div>
        <br/><hr/>
        <asp:Label runat="server" ID="Message"></asp:Label>
    </form>
</body>
</html>
