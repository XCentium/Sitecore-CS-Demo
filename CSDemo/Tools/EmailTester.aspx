<%@ Page Language="C#" %>
<%@ Import Namespace="Sitecore" %>
<%@ Import Namespace="System.Net.Mail" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title>Email Test</title>    
</head>
<body>
    <script runat="server">
        protected void SendEmail(object sender, EventArgs e)
        {
            const string from = "web.dev@xcentium.com";
            const string to = "john.montes@xcentium.com";
            const string subject = "Test Email from CS Demo";
            const string body = "BODY! Test Email from CS Demo";
        
            var emailMessage = new MailMessage(from, to, subject, body);
            MainUtil.SendMail(emailMessage);
            lblResult.Text = "Sent!";
        }
    </script>
    <form id="form1" runat="server">   
        <asp:Button runat="server" OnClick="SendEmail" Text="Send" />
        <asp:Label runat="server" ID="lblResult" Text=""></asp:Label>
    </form>
</body>
</html>