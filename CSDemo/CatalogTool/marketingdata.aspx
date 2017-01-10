<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace ="System" %>
<%@ Import Namespace ="System.Collections.Generic" %>
<%@ Import Namespace ="System.Linq" %>
<%@ Import Namespace ="System.Web" %>
<%@ Import Namespace ="System.Web.UI" %>
<%@ Import Namespace ="System.Web.UI.WebControls" %>
<%@ Import Namespace ="Sitecore" %>
<%@ Import Namespace ="Sitecore.Data" %>
<%@ Import Namespace ="Sitecore.Links" %>
<%@ Import Namespace ="Sitecore.Security" %>
<%@ Import Namespace ="Sitecore.SecurityModel" %>
<%@ Import Namespace ="Sitecore.Data.Items" %>
<%@ Import Namespace ="Sitecore.Data.Fields" %>

<script runat="server" >

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGetPlan_Click(object sender, EventArgs e)
    {
        // get the id

        String planID = txtPlanID.Text.Trim();

        String strTempName = txtChildTemplateName.Text.Trim();

        // if ID, get the plan XML
        if (planID != "")
        {

            // Item planItem = Sitecore.Context.Database.GetItem(new ID(planID));
            IEnumerable<Item> planItem = Sitecore.Context.Database.GetItem(new ID(planID)).GetChildren();
            // IEnumerable<Item> planItem = Sitecore.Context.Database.GetItem(new ID(planID)).Axes.GetDescendants().Where(x => x.TemplateName == "Comment");
          //  IEnumerable<Item> planItem = Sitecore.Context.Database.GetItem(new ID(planID)).Axes.GetDescendants().Where(x => x.TemplateName == strTempName);

            rptItem.DataSource = planItem;
            rptItem.DataBind();

            // String planXML = getPlanXML(planID);

            // Writeout the xml
            // Response.Write(planXML);

        }
        else
        {
            Response.Write("Provide a valid ID");
        }

    }



</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Home ID:
        <asp:TextBox ID="txtPlanID" runat="server" Width="355px">{4A54BEE9-E1EF-4F43-B6A0-9E494A16E309}</asp:TextBox>
        <br />
        Child TemplateName:
        <asp:TextBox ID="txtChildTemplateName" runat="server" Width="282px">Comment</asp:TextBox>
        <br/>
        <asp:Button ID="btnGetPlan" runat="server" OnClick="btnGetPlan_Click" Text="Get Plan XML" />
    
        <br />
        <br />
        <asp:Repeater ID="rptItem" ItemType="Sitecore.Data.Items.Item" runat="server">
            <HeaderTemplate>
                <div class="indentedSection">
	                <h3>Item</h3>
            </HeaderTemplate>
            <ItemTemplate>
                <p>ID:<%#: Item.ID %></p>
                <p>Name:<%#: Item.Name %></p>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    
    </div>
    </form>
</body>
</html>
