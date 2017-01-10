<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogSoap.aspx.cs" Inherits="CSDemo.CatalogTool.CatalogSoap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     Soap response:   <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <hr/>
        <asp:Button ID="btnGetCatalogs" runat="server" Text="Get Catalogs" OnClick="btnGetCatalogs_Click" />
        <hr/>
    </div>
    </form>
</body>
</html>
