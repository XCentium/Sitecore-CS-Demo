<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Publishing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script runat="server">

        protected void Page_Load(object sender, EventArgs e)
        {
            string full = Request.QueryString["full"];

            // Set up the publish mode
            PublishMode publishMode = PublishMode.Smart;
            if (!
                string
                    .
                    IsNullOrWhiteSpace(full)
                && (
                    full
                    == "1" ||
                    full.Equals
                        ("true",
                            StringComparison.InvariantCultureIgnoreCase
                        )))
            {
                publishMode = PublishMode.Full;
            }

            using (

                new
                    Sitecore.SecurityModel.SecurityDisabler())
            {
                //target database
                var webDb = Sitecore.Configuration.Factory.GetDatabase("web");

                //source database
                var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");

                try
                {
                    foreach (Language language in masterDb.Languages)
                    {
                        //loops on the languages and do a full republish on the whole sitecore content tree
                        var options = new PublishOptions(masterDb, webDb, publishMode, language, DateTime.Now)
                        {RootItem = masterDb.Items["/sitecore"], RepublishAll = true, Deep = true};
                        var myPublisher = new Publisher(options);
                        myPublisher.Publish();
                    }
                }
                catch (
                    Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error
                        ("Could not publish the master database to the web",
                            ex
                        );
                    throw ex;
                }

            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
