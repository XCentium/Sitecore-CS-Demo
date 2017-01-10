<%@ Page Language="C#" AutoEventWireup="true"  Debug="true" %>
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

<!DOCTYPE html>
<script runat="server">


    protected void btngetItemCSV_Click(object sender, EventArgs e)
    {
        // disable security 
        using (new SecurityDisabler())
        {


            // get the item ID or poath
            String itemIDorPath = txtID.Text.Trim();
            
            // determine if it is a path or ID
            if (itemIDorPath.IndexOf('/') > 0)
            {

                // this is a path, process path
                processPath(itemIDorPath);

            }

            // determine if it is a path or ID
            if (itemIDorPath.IndexOf('}') > 0)
            {

                // this is an ID , process GUID
                processGuid(itemIDorPath);

            }            
           
 
            
        }
    }

    private void processPath(string path) {

        // select the item, if it is null,,say it is null, else, list its fields
        Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase(ddDatabase.SelectedValue.ToString().Trim().ToLower());

        String strParentPath = "/sitecore/content" + formatPath(path.Trim()) + "#";
        
        Item currentItem = db.GetItem(path);

        if (currentItem != null)
        {

            listItemFields(currentItem);

            bindItems();
        }
        else
        {

            say("NULL ITEM", "2");
        }                
    
    }

    private void processGuid(string guid)
    {

        // select the item, if it is null,,say it is null, else, list its fields
        Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase(ddDatabase.SelectedValue.ToString().Trim().ToLower());

        Item currentItem = db.GetItem(new ID(guid));

        if (currentItem != null)
        {

            listItemFields(currentItem);

            bindItems();
        }
        else {

            say("NULL ITEM", "2");
        }

    }

    private void listItemFields(Item currentItem)
    {


        List<string> fieldNames = new List<string>();
        currentItem.Fields.ReadAll();
        Sitecore.Collections.FieldCollection fieldCollection = currentItem.Fields;

        Response.Write("<h2>Standard Fields</h2><hr/>");
        Response.Write("<table align='left' border='1' cellspacing='2' cellpadding='2' width='100%'><tr align='left' valign='top'><th nowrap='nowrap'>Field</th><th nowrap='nowrap'>Value</th></tr>");

        foreach (Field field in fieldCollection)
        {
            //Use the following check if you do not want 
            //the Sitecore Standard Fields 

            if (field.Name.StartsWith("__"))
            {
                String fname = field.Name.ToString();
                // Response.Write(fname + " : value = " + currentItem[fname].ToString() + "<hr />");
                Response.Write("<tr align='left' valign='top' nowrap='nowrap'>	<td>" + fname + "</td><td>" + currentItem[fname].ToString() + "</td></tr>");
            }

        }
        Response.Write("</table>");


        Response.Write("<h2>Other Fields</h2><hr/>");
        Response.Write("<table align='left' border='1' cellspacing='2' cellpadding='2' width='100%'><tr align='left' valign='top'><th nowrap='nowrap'>Field</th><th nowrap='nowrap'>Value</th></tr>");

        foreach (Field field in fieldCollection)
        {
            //Use the following check if you do not want 
            //the Sitecore Standard Fields 
            if (!field.Name.StartsWith("__"))
            {
                String fname = field.Name.ToString();
                Response.Write("<tr align='left' valign='top'>	<td nowrap='nowrap'>" + fname + "</td><td>" + currentItem[fname].ToString() + "</td></tr>");
            }

        }
        Response.Write("</table>");
                           
    
    }

    private void bindItems() {

        string itemID = txtID.Text.Trim();
        
        // Select database to query from drop down list
        Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase(ddDatabase.SelectedValue.ToString().Trim().ToLower());

        // select the item or items as a list so it can be binded.
        IEnumerable<Item> selectedtems = db.SelectItems("fast://*[@@id = '" + itemID + "']");

        // bind the item
        rptItem.DataSource = selectedtems;
        rptItem.DataBind();
               
    }

    private void say(string txt, string h) {

        Response.Write("<" + h + ">" + txt +"</" + h + ">");
    }

    public String formatPath(String path)
    {
        path = path.Replace("/sitecore/content", "");
        String output = "|||" + path;

        output = output.Replace("/", "#/#");
        output = output.Replace("|||#/", "/");

        return output;

    }

    protected void btnAllPromotions_Click(object sender, EventArgs e)
    {
        // get all metro-pages
        // loop through and get the child with template openhtml that has path all-promotions
        // add the guid, path, published status, 
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pull all sales counsellors</title>
    <style>

        html, body, h1, form, fieldset, legend, ol, li {
	        margin: 0;
	        padding: 0;
        }
        body {
	        background: #ffffff;
	        color: #111111;
	        font-family: Georgia, "Times New Roman", Times, serif;
	        padding: 20px;
        }

	h1 {
		font-size: 28px;
		margin-bottom: 20px;
		}
	
	#content {
		background: #9cbc2c;
		-moz-border-radius: 5px;
		-webkit-border-radius: 5px;
		-khtml-border-radius: 5px;
		border-radius: 5px;
		counter-reset: fieldsets;
		padding: 20px;
		mim-width: 400px;
        margin-left: auto ;
        margin-right: auto ; 
		}
		
	form fieldset {
		border: none;
		margin-bottom: 10px;
	}
		
	form fieldset:last-of-type {
		margin-bottom: 0;
	}
			
	form legend {
		color: #384313;
		font-size: 16px;
		font-weight: bold;
		padding-bottom: 10px;
		text-shadow: 0 1px 1px #c0d576;
	}
				
	form > fieldset > legend:before {
		content: "Step " counter(fieldsets) ": ";
		counter-increment: fieldsets;
	}
	
	form fieldset fieldset legend {
		color: #111111;
		font-size: 13px;
		font-weight: normal;
		padding-bottom: 0;
	}
			
	form ol li {
		background: #b9cf6a;
		background: rgba(255,255,255,.3);
		border-color: #e3ebc3;
		border-color: rgba(255,255,255,.6);
		border-style: solid;
		border-width: 2px;
		-moz-border-radius: 5px;
		-webkit-border-radius: 5px;
		-khtml-border-radius: 5px;
		border-radius: 5px;
		line-height: 30px;
		list-style: none;
		padding: 5px 10px;
		margin-bottom: 2px;
	}
							
	form ol ol li {
		background: none;
		border: none;
		float: left;
	}
			
	form label {
		float: left;
		font-size: 13px;
		width: 265px;
	}
				
	form fieldset fieldset label {
		background:none no-repeat left 50%;
		line-height: 20px;
		padding: 0 0 0 30px;
		width: auto;
	}
			

				
	form input:not([type=radio]),
	form textarea {
		background: #ffffff;
		border: none;
		-moz-border-radius: 3px;
		-webkit-border-radius: 3px;
		-khtml-border-radius: 3px;
		border-radius: 3px;
		font: italic 13px Georgia, "Times New Roman", Times, serif;
		outline: none;
		padding: 5px;
		width: 200px;
	}
					
	form input:not([type=submit]):focus,
	form textarea:focus {
		background: #eaeaea;
	}

	form input[type=radio] {
		float: left;
		margin-right: 5px;
	}
						
	form #btnAllPromotions,  form #btnSoldQMIs   {
		background: #384313;
		border: none;
		-moz-border-radius: 20px;
		-webkit-border-radius: 20px;
		-khtml-border-radius: 20px;
		border-radius: 20px;
		color: #ffffff;
		display: block;
		font: 18px Georgia, "Times New Roman", Times, serif;
		letter-spacing: 1px;
		margin: auto;
		padding: 7px 7px;
		text-shadow: 0 1px 1px #000000;
		text-transform: uppercase;
	}
			
	form button:hover {
		background: #1e2506;
		cursor: pointer;
	}
    
    body {
    background-color: white !important;
    }    
    
       .rptBox {
            width:150px;
            margin-left:10px;
            padding: 5px;
        }
    	

       </style>

</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
    <fieldset>
        <legend>List Items</legend>
        <ol>
            <li>
                <label for="ddDatabase">Database Source</label><br />
                    <asp:DropDownList ID="ddDatabase" runat="server" AutoPostBack="True">
                        <asp:ListItem>Master</asp:ListItem>
                        <asp:ListItem>Web</asp:ListItem>
                        <asp:ListItem>Delivery</asp:ListItem>
                        <asp:ListItem>Core</asp:ListItem>
                    </asp:DropDownList>
            </li>

                <li><asp:Button ID="btnAllPromotions" CssClass="btn" runat="server" Text="Pull All Promotions" OnClick="btnAllPromotions_Click" /></li>

                <li><asp:Button ID="btnSoldQMIs" CssClass="btn" runat="server" Text="Pull All Sold QMIs" /></li>


                <li><asp:Button ID="btngetItemCSV" CssClass="btn" runat="server" Text="Get Items" OnClick="btngetItemCSV_Click" /></li>
            
        </ol>

       
    </fieldset>
        <hr/>
 
    &nbsp;</div>
       <br />
        <asp:Repeater ID="rptItem" ItemType="Sitecore.Data.Items.Item" runat="server">
            <HeaderTemplate>
                <div class="indentedSection">
	                <h3>Item List</h3>
                <table style="width:100%;">
                    <tr>
                        <th>&nbsp;ID</th>
                        <th>&nbsp;Name</th>
                        <th>&nbsp;Path</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>&nbsp;ID:<%#: Item.ID %></td>
                    <td>&nbsp;Name:<%#: Item.Name %></td>
                    <td>&nbsp;Path:<%#: Item.Paths.ContentPath %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
 
    </form>
</body>
</html>
