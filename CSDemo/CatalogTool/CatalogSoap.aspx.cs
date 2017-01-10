using System;
using System.IO;
using System.Net;

namespace CSDemo.CatalogTool
{
    public partial class CatalogSoap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetCatalogs_Click(object sender, EventArgs e)
        {
            var envelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <GetCatalogs xmlns=""http://schemas.commerceserver.net/2013/01/CatalogWebService"">
      <searchClause></searchClause>
      <language>En</language>
      <searchOptions>
        <ClassTypes>None</ClassTypes>
      </searchOptions>
    </GetCatalogs>
  </soap12:Body>
</soap12:Envelope>
";
            HttpWebRequest req =
                (HttpWebRequest)WebRequest.Create("http://CSSolutionStorefrontsite_CatalogWebService/CatalogWebService.asmx");
            req.Headers.Add(String.Format("SOAPAction: \"{0}\"", "http://tempuri.org/Register"));
            req.ContentType = "application/soap+xml; charset=utf-8";
            req.Accept = "text/xml";
            req.Method = "POST";
            var username = "";
            var password = "";
            var domain = "";
            req.Credentials = new NetworkCredential(username, password, domain);


            using (Stream stm = req.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(envelope);
                }
            }

            WebResponse response = req.GetResponse();

            // Stream responseStream = response.GetResponseStream();
            // ReSharper disable once AssignNullToNotNullAttribute
            StreamReader srd = new StreamReader(response.GetResponseStream());

            // TODO: Do whatever you need with the response // Label1

            Label1.Text = srd.ReadToEnd();
            srd.Close();

        }
    }
}