<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getItemChildren.aspx.cs" Inherits="Training.getItemChildren" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label Text="Input Parent ID" runat="server" />:<asp:TextBox ID="txtParentID" runat="server">{4A54BEE9-E1EF-4F43-B6A0-9E494A16E309}</asp:TextBox>
&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtParentID" ErrorMessage="ID Entry Required"></asp:RequiredFieldValidator>
        <asp:Button ID="btnGetChildren" runat="server" OnClick="btnGetChildren_Click" Text="Get children" />
        <hr />
        <asp:Repeater ID="rptChildren" runat="server" />
        
    
    </div>
    </form>
</body>
</html>
