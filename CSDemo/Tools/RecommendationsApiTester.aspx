<%@ page language="C#" %>

<%@ import namespace="Sitecore" %>
<%@ import namespace="System.Net.Mail" %>
<%@ import namespace="CSDemo.Helpers" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Recommendations Api Tester</title>
</head>
<body>
    <script runat="server">
        protected void GetItemRecommendations(object sender, EventArgs e)
        {
            var space = "\r\n";
            var itemid = txtItemId.Text;

           var items = RecommendationsHelper.GetItemRecommendations(itemid, 10); 
        
            if (items == null)
               return;
            
            txtResults.Text = "";
            
            var result = new StringBuilder("Showing Item Recommendations for item id: " + itemid);
            result.Append(space);
            result.Append("--------------");
            result.Append(space);

            if (items.ItemsIds != null)
            {
                foreach (var itemId in items.ItemsIds)
                {
                    result.Append(itemId);
                    result.Append(space);
                }
            }
            else
            {
                result.Append("NO ITEMS FOUND.");
                result.Append(space);
            }

            result.Append("---- END ----");
            txtResults.Text = result.ToString();
        }

        protected void GetUserRecommendations(object sender, EventArgs e)
        {
            var space = "\r\n";
            var userId = txtUserId.Text;

            var items = RecommendationsHelper.GetUserRecommendations(userId, 10);

            if (items == null)
                return;
            
            txtResults.Text = "";
            
            var result = new StringBuilder("Showing Item Recommendations for user id: " + userId);
            result.Append(space);
            result.Append("--------------");
            result.Append(space);

            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    result.Append(item.Id);
                    result.Append(space);
                }
            }
            else
            {
                result.Append("NO ITEMS FOUND.");
                result.Append(space);
            }

            result.Append("---- END ----");
            txtResults.Text = result.ToString();
        }

        protected void GetFbtRecommendations(object sender, EventArgs e)
        {
            var space = "\r\n";
            var itemid = txtItemId.Text;

            var items = RecommendationsHelper.GetFrequentlyBoughtTogetherRecommendations(itemid, 10);

            if (items == null)
                return;
            
            txtResults.Text = "";
            
            var result = new StringBuilder("Showing FBT Recommendations for item id: " + itemid);
            result.Append(space);
            result.Append("--------------");
            result.Append(space);

            if (items.ItemsIds != null)
            {
                foreach (var itemId in items.ItemsIds)
                {
                    result.Append(itemId);
                    result.Append(space);
                }
            }
            else
            {
                result.Append("NO ITEMS FOUND.");
                result.Append(space);
            }

            result.Append("---- END ----");
            txtResults.Text = result.ToString();
        }
    </script>
    <form id="form1" runat="server">
        <strong>Item Id:</strong><br />
        <asp:textbox runat="server" id="txtItemId" width="100"></asp:textbox>
        <br />
        <strong>User Id:</strong><br />
        <asp:textbox runat="server" id="txtUserId" width="100"></asp:textbox>
        <br />
        <br />

        <asp:button runat="server" onclick="GetItemRecommendations" text="Get Item Recommendations" />
        <br /><br />
        <asp:button runat="server" onclick="GetUserRecommendations" text="Get User Recommendations" />
        <br /><br />
        <asp:button runat="server" onclick="GetFbtRecommendations" text="Get FBT Recommendations" />
        <br />
        <br />
        <strong>Results:</strong>
        <br />
        <asp:textbox runat="server" id="txtResults" textmode="MultiLine" height="600" width="600"></asp:textbox>

        <asp:label runat="server" id="lblResult" text=""></asp:label>
    </form>
</body>
</html>
