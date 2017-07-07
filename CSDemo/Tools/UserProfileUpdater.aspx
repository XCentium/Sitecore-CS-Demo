<%@ page language="C#" %>

<%@ import namespace="Sitecore" %>
<%@ import namespace="System.Net.Mail" %>
<%@ import namespace="CSDemo.Helpers" %>
<%@ Import Namespace="CSDemo.Models.Account" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Recommendations Api Tester</title>
</head>
<body>
    <script runat="server">
        protected void SetUserProfileProperty(object sender, EventArgs e)
        {
            var userId = txtUserId.Text;
            var username = txtUserName.Text;

            var user = Sitecore.Security.Accounts.User.FromName(username, true);
            var profile = user.Profile;
            user.Profile.SetCustomProperty("scommerce_customer_id", userId);
            user.Profile.Save();

            txtResults.Text = "SetUserProfileProperty... DONE!";
        }

        protected void GetUserProfileProperty(object sender, EventArgs e)
        {
            var space = "\r\n";
            var username = txtUserName.Text;

            var result = new StringBuilder("GetUserProfileProperty for: " + username);
            result.Append(space);

            var id = AccountHelper.GetCommerceUserId(username);
            
            if (id == null)
                result.Append("id == null");
            else
                result.Append("id == " + id.ToString());
        
            result.Append(space);
            result.Append("---- END ----");

            txtResults.Text = result.ToString();
        }
    </script>
    <form id="form1" runat="server">
        <strong>New User Id:</strong><br />
        <asp:textbox runat="server" id="txtUserId" width="100"></asp:textbox>
        <br />
        <strong>User Name:</strong><br />
        <asp:textbox runat="server" id="txtUserName" width="100"></asp:textbox>
        <br />
        <br />

        <asp:button runat="server" onclick="SetUserProfileProperty" text="Set User Profile Property" />
        <br /><br />
        <asp:button runat="server" onclick="GetUserProfileProperty" text="Get User Profile Property" />
        <br /><br />
        <strong>Results:</strong>
        <br />
        <asp:textbox runat="server" id="txtResults" textmode="MultiLine" height="600" width="600"></asp:textbox>

        <asp:label runat="server" id="lblResult" text=""></asp:label>
    </form>
</body>
</html>
