<%@ page language="C#" %>

<%@ import namespace="Sitecore" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>XC Visual Merchandiser v0.1 Alpha</title>

    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

    <style>
        body {
            min-width: 1000px;
            width: auto !important;
            width: 1000px;
        }

        #products {
            width: 100%;
        }

        .product {
            margin: 15px 0;
            cursor: move;
        }

            .product .description {
                text-overflow: ellipsis;
                overflow: hidden;
                white-space: nowrap;
            }

            .product .price .amount{
                text-decoration: none;
            }

        .product-container {
            box-shadow: 2px 2px 2px 2px rgba(0,0,0,0.1);
            max-width: 300px;
            padding: 10px;
        }

        .chosen .product-container {
            border: solid 2px red;
        }

        .ghost .product-container {
            opacity: .5;
            background: #C8EBFB;
            border: dashed 5px red;
        }
    </style>
</head>
<body>
    <nav class="navbar navbar-default">
        <div class="container-fluid">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#">XCVM</a>
            </div>

            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <div style="width: 100%;">
                    <span style="float: left; display: block; line-height: 50px; padding: 0 20px; font-weight: bold;">Categories:</span>
                    <select id="cmbCategories" clientidmode="Static" runat="server" class="form-control" style="width: 100%; max-width: 300px; text-align: right; margin-top: 10px; float: left;">
                    </select>
                    <button class="btn btn-default" style="margin: 10px; width: 100px;" id="btnSave">Save</button>
                    <div class="clearfix"></div>
                </div>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>
    <div class="container" style="max-width: 1000px;">
        <div class="row">
            <div id="products">
                <%--<div class="product col-sm-4" data-id="dubes-1">
                    <div class="product-container">
                        <div class="col-sm-12">
                            <div class="product-overlay">
                                <div class="product-mask"></div>
                                <a href="/categories/boots/aw074-04" class="product-permalink" target="_blank"></a>
                                <img src="http://csdemo.local/-/media/images/adventure-works/boot01_1.png?as=1&h=287&w=420&bc=ffffff&hash=7DE8A9884C2D63443F3636182E188722EB87BFBB" class="img-responsive product-image-1" alt="">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="product-body">
                                <h3>Dunes 1</h3>
                                <div class="product-labels">
                                </div>

                                <div class="product-rating">
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star-o"></i>
                                </div>
                                <span class="price">
                                    <ins><span id="productAmount_AW074_04" class="amount">$98.00</span></ins>
                                </span>
                                <p>Lightweight nylon/suede hiking boots.</p>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
                <div class="product col-sm-4" data-id="dubes-2">
                    <div class="product-container">
                        <div class="col-sm-12">
                            <div class="product-overlay">
                                <div class="product-mask"></div>
                                <a href="/categories/boots/aw074-04" class="product-permalink" target="_blank"></a>
                                <img src="http://csdemo.local/-/media/images/adventure-works/boot01_1.png?as=1&h=287&w=420&bc=ffffff&hash=7DE8A9884C2D63443F3636182E188722EB87BFBB" class="img-responsive product-image-1" alt="">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="product-body">
                                <h3>Dunes 2</h3>
                                <div class="product-labels">
                                </div>

                                <div class="product-rating">
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star-o"></i>
                                </div>
                                <span class="price">
                                    <ins><span id="productAmount_AW074_04" class="amount">$98.00</span></ins>
                                </span>
                                <p>Lightweight nylon/suede hiking boots.</p>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
                <div class="product col-sm-4" data-id="dubes-3">
                    <div class="product-container">
                        <div class="col-sm-12">
                            <div class="product-overlay">
                                <div class="product-mask"></div>
                                <a href="/categories/boots/aw074-04" class="product-permalink" target="_blank"></a>
                                <img src="http://csdemo.local/-/media/images/adventure-works/boot01_1.png?as=1&h=287&w=420&bc=ffffff&hash=7DE8A9884C2D63443F3636182E188722EB87BFBB" class="img-responsive product-image-1" alt="">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="product-body">
                                <h3>Dunes 3</h3>
                                <div class="product-labels">
                                </div>

                                <div class="product-rating">
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star-o"></i>
                                </div>
                                <span class="price">
                                    <ins><span id="productAmount_AW074_04" class="amount">$98.00</span></ins>
                                </span>
                                <p>Lightweight nylon/suede hiking boots.</p>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>

                <div class="product col-sm-4" data-id="dubes-4">
                    <div class="product-container">
                        <div class="col-sm-12">
                            <div class="product-overlay">
                                <div class="product-mask"></div>
                                <a href="/categories/boots/aw074-04" class="product-permalink" target="_blank"></a>
                                <img src="http://csdemo.local/-/media/images/adventure-works/boot01_1.png?as=1&h=287&w=420&bc=ffffff&hash=7DE8A9884C2D63443F3636182E188722EB87BFBB" class="img-responsive product-image-1" alt="">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="product-body">
                                <h3>Dunes 4</h3>
                                <div class="product-labels">
                                </div>

                                <div class="product-rating">
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star-o"></i>
                                </div>
                                <span class="price">
                                    <ins><span id="productAmount_AW074_04" class="amount">$98.00</span></ins>
                                </span>
                                <p>Lightweight nylon/suede hiking boots.</p>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>

                <div class="product col-sm-4" data-id="dubes-5">
                    <div class="product-container">
                        <div class="col-sm-12">
                            <div class="product-overlay">
                                <div class="product-mask"></div>
                                <a href="/categories/boots/aw074-04" class="product-permalink" target="_blank"></a>
                                <img src="http://csdemo.local/-/media/images/adventure-works/boot01_1.png?as=1&h=287&w=420&bc=ffffff&hash=7DE8A9884C2D63443F3636182E188722EB87BFBB" class="img-responsive product-image-1" alt="">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="product-body">
                                <h3>Dunes 5</h3>
                                <div class="product-labels">
                                </div>

                                <div class="product-rating">
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star-o"></i>
                                </div>
                                <span class="price">
                                    <ins><span id="productAmount_AW074_04" class="amount">$98.00</span></ins>
                                </span>
                                <p>Lightweight nylon/suede hiking boots.</p>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>

    <div id="template" style="display: none">
        <div class="product col-sm-4" data-id="{ProductId}">
            <div class="product-container">
                <div class="col-sm-12">
                    <div class="product-overlay">
                        <div class="product-mask"></div>
                        <a href="{Url}" class="product-permalink" target="_blank"></a>
                        <img src="{Image}?as=1&h=420&w=420" class="img-responsive" alt="">
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="product-body">
                        <h3 style="height: 52px;">{Title}</h3>
                        <div class="product-labels">
                        </div>

                        <div class="product-rating">
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star-o"></i>
                        </div>
                        <span class="price">
                            <ins><span id="productAmount_AW074_04" class="amount">{Price}</span></ins>
                        </span>
                        <p class="description">{Description}</p>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
    </div>

    <script src="https://code.jquery.com/jquery-3.2.1.min.js" integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Sortable/1.5.1/Sortable.min.js"></script>

    <script runat="server">
        protected override void OnLoad(EventArgs e)
        {
            var categories = CSDemo.Models.Product.ProductHelper.GetCategories().OrderBy(c => c.Name);
            cmbCategories.Items.Clear();

            var ctr = 0;
            foreach (var category in categories)
            {
                cmbCategories.Items.Add(new ListItem{Text = category.Name, Value = category.ID.ToString(), Selected = (ctr == 0)});
                ctr++;
            }

            base.OnLoad(e);
        }
    </script>

    <script>
        var vm = {
            sortable: null,
            products: null,
            GetCategoryId: function () { return $("#cmbCategories").val();  },
            LoadCategoryProducts: function () {
                var selectedCategoryId = vm.GetCategoryId();

                if (selectedCategoryId !== "" && selectedCategoryId != null) {
                    $.ajax({
                        type: "POST",
                        url: "/AJAX/cart.asmx/GetProductsByCategory",
                        data: JSON.stringify({ categoryId: selectedCategoryId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (result) {
                            vm.products = JSON.parse(result.d);
                        },
                        error: function (error) {
                            console.log(error);
                            vm.products = "";
                        },
                        complete: vm.DisplayCategoryProducts
                    });
                }
            },
            DisplayCategoryProducts: function () {
                $("#products").empty();

                if (vm.products != null && vm.products.length > 0) {
                    var products = "";
                    for (var ctr = 0; ctr < vm.products.length; ctr++) {
                        var prod = vm.products[ctr];
                        var prodImage = prod.Images != null && prod.Images.length > 0 ? prod.Images[0].Src : "";
                        var newProduct = $("#template").html();

                        newProduct = newProduct.replace("{ProductId}", prod.ProductId);
                        newProduct = newProduct.replace("{Image}", prodImage);
                        newProduct = newProduct.replace("{Url}", prod.Url);
                        newProduct = newProduct.replace("{Title}", prod.Title);
                        newProduct = newProduct.replace("{Price}", prod.Price.toFixed(2));
                        newProduct = newProduct.replace("{Description}", prod.Description);

                        products += newProduct;
                    }

                    $("#products").html(products);
                    vm.InitSortable();
                }
            },
            SaveProductPositions: function () {
                alert(vm.sortable.toArray());
            },
            InitSortable: function () {
                //init sortable grid
                var options = {
                    animation: 150, // ms, animation speed moving items when sorting, `0` — without animation
                    handle: ".product-container", // Restricts sort start click/touch to the specified element
                    draggable: ".product", // Specifies which items inside the element should be sortable
                    onUpdate: function (evt /**Event*/) {
                        var item = evt.item; // the current dragged HTMLElement
                    },
                    chosenClass: "chosen",
                    ghostClass: 'ghost'
                };

                vm.sortable = Sortable.create(products, options);
            },
            Init: function () {
                //event handlers
                $("#btnSave").on("click", function () {
                    vm.SaveProductPositions();
                });

                $("#cmbCategories").on("change", function () {
                    vm.LoadCategoryProducts();
                });

                vm.LoadCategoryProducts();
            }
        };

        $("document").ready(function () {
            vm.Init();
        });
    </script>
</body>
</html>
