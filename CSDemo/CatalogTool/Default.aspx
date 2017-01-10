<%@ page language="C#" autoeventwireup="true" codebehind="Default.aspx.cs" inherits="CSDemo.CatalogTool.Default" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <script>
        window.jQuery ||
        document.write("<script src='http://code.jquery.com/jquery-1.11.0.min.js'>\x3C/script>")
    </script>

    <script type="text/javascript">

        $(document).ready(function () {

        });
    </script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <style>
        th, td {
            padding: 10px;
            text-align: left;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">



        <div class="container">
            <h1>My First Bootstrap Page</h1>
            <p>This is some text.</p>
            <table style="border-collapse: collapse; border-spacing: 2px; border-padding: 2px; table-layout: auto">
                <tr>
                    <td>CatalogRoot:</td>
                    <td>
                        <asp:DropDownList ID="ddlCatalogRoot" runat="server" OnSelectedIndexChanged="ddlCatalogRoot_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Catalog:</td>
                    <td>
                        <asp:DropDownList ID="ddlCatalog" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCatalog_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>

                <tr>
                    <td>Categories:</td>
                    <td>
                        <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged"></asp:DropDownList>
                        
                    </td>
                </tr>
                <tr>
                    <td>Products Types:</td>
                    <td>
                        <asp:DropDownList ID="ddlProductTypes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProductTypes_SelectedIndexChanged"></asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td>Products:</td>
                    <td>
                        <asp:DropDownList ID="ddlProducts" runat="server" AutoPostBack="True"></asp:DropDownList>
                        <%--<br/> <asp:LinkButton ID="LinkButton4" runat="server">Download all listed products</asp:LinkButton>--%>
                    </td>
                </tr>
                <tr>
                    <td>Product Variants:</td>
                    <td>
                        <asp:DropDownList ID="ddlProductVariants" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProductVariants_SelectedIndexChanged"></asp:DropDownList>
                        <br/> 

                    </td>
                </tr>
            </table>
            <asp:TextBox ID="txtId" runat="server"></asp:TextBox>
            <asp:Button ID="btnGetItemProperties" runat="server" Text="Get Item Properties" OnClick="btnGetItemProperties_Click" />
            <hr />

            <ul class="nav nav-tabs">

                <li class="active"><a data-toggle="tab" href="#category">Category</a></li>
                <li><a data-toggle="tab" href="#product">Product</a></li>
                <li><a data-toggle="tab" href="#variants">Product variants</a></li>
            </ul>

            <div class="tab-content">
 
                                
                <div id="category" class="tab-pane fade active in">
                    <div class="panel panel-default">
                      <div class="panel-heading">
                        <h3 class="panel-title">Category Operations</h3>
                      </div>
                      <div class="panel-body">
                        <b>Selected : 
                            <asp:Label ID="lblCategory" runat="server" Text="Label"></asp:Label>
                        </b>
                      </div>
                     <!-- List group -->
                      <ul class="list-group">
                        <li class="list-group-item"><asp:LinkButton ID="lnkDownloadCSVTemplateCategory" runat="server" OnClick="lnkDownloadCSVTemplateCategory_Click">Download CSV Template for Categories Upload</asp:LinkButton></li>                          
                        <li class="list-group-item"><asp:LinkButton ID="lnkDownloadAllCategories" runat="server" OnClick="lnkDownloadAllCategories_Click">Download all categories</asp:LinkButton></li>
                        <li class="list-group-item">Dapibus ac facilisis in</li>
                        <li class="list-group-item">Morbi leo risus</li>
                        <li class="list-group-item">Porta ac consectetur ac</li>
                        <li class="list-group-item">Vestibulum at eros</li>
                      </ul>
                    </div>
                </div>
                <div id="product" class="tab-pane fade">
                    <div class="panel panel-default">
                      <div class="panel-heading">
                        <h3 class="panel-title">Product Operations</h3>
                      </div>
                      <div class="panel-body">
                        <b>Selected : 
                            <asp:Label ID="lblProduct" runat="server" Text="Label"></asp:Label>
                        </b>

                      </div>
                     <!-- List group -->
                      <ul class="list-group">
                        <li class="list-group-item"><asp:LinkButton ID="lnkDownloadCSVTemplateProductType" Enabled="False" runat="server" OnClick="lnkDownloadCSVTemplateProductType_Click">Download CSV Template for product type <asp:Label ID="lblProductTypeDownload" runat="server" Text="Label"> Upload</asp:Label> </asp:LinkButton> </li>
                        <li class="list-group-item">Dapibus ac facilisis in</li>
                        <li class="list-group-item">Morbi leo risus</li>
                        <li class="list-group-item">Porta ac consectetur ac</li>
                        <li class="list-group-item">Vestibulum at eros</li>
                      </ul>
                    </div>
                </div>
                <div id="variants" class="tab-pane fade">
                    <div class="panel panel-default">
                      <div class="panel-heading">
                        <h3 class="panel-title">Product Variants Operations</h3>
                      </div>
                      <div class="panel-body">
                        <b>Selected : 
                            <asp:Label ID="lblVariants" runat="server" Text="Label"></asp:Label>
                        </b>
                      </div>
                     <!-- List group -->
                      <ul class="list-group">
                        <li class="list-group-item"><asp:LinkButton ID="lnkDownloadCSVTemplateProductVariantType" runat="server" OnClick="lnkDownloadCSVTemplateProductVariantType_Click">Download CSV Template for product variant type <asp:Label ID="lblProductVariantTypeDownload" runat="server" Text="Label"> Upload</asp:Label> </asp:LinkButton></li>
                        <li class="list-group-item">Dapibus ac facilisis in</li>
                        <li class="list-group-item">Morbi leo risus</li>
                        <li class="list-group-item">Porta ac consectetur ac</li>
                        <li class="list-group-item">Vestibulum at eros</li>
                      </ul>
                    </div>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
