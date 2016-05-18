(function (window, $, undefined) {

    //
    var commerceActionAllowed = false;  // This variable will determine if the user can perform commerce actions
   

    window.ProductComparisonCookieName = "ComparisonProducts";
    $(document).ready(function () {

        displayPersonalizedCoupon();
        isUserLoggedInCommerceUser();
        
        checkIfCommerceActionsAllowed();
        loadCheckoutFormData();
        loadPaymentFormData();
        loadShippingFormData();
        loadReviewFormData();
        loadOrderConfirmationData();
        adjustVariantCarousel();
        setProductComparison();

        console.log(document.cookie);
    });

    function displayPersonalizedCoupon() {

        var showCoupon = Cookies.get("ShowCoupon");
        if (typeof showCoupon != 'undefined' && showCoupon === "true") {

            var couponMessage = Cookies.get("CouponMessage");
            if (typeof couponMessage != 'undefined' && couponMessage !== "|") {
                if (couponMessage != null) {

                    var couponMessages = couponMessage.split('|');
                    var message = "<h2>" + couponMessages[1] + "</h2>";
                    message += "<p>" + couponMessages[0] + "</p>";
                    showActionMessageFixed(message);
                    Cookies.set("ShowCoupon", "false");
                }
            }

        }
    }

    function isUserLoggedInCommerceUser() {

        clearSessionTimeOutCookies();

        var userLoggedInAsCommerceUser = Cookies.get("CommerceUserLoggedIn");

        if (typeof userLoggedInAsCommerceUser != 'undefined' && userLoggedInAsCommerceUser === "true") {
            
            var selectedCatalog = Cookies.get("UserSelectedCatalogId");
            if (typeof selectedCatalog != 'undefined' && selectedCatalog === "") {

                var catalogOptions = Cookies.get("UserCatalogOptions");
                if (typeof catalogOptions != 'undefined' && catalogOptions !== "") {

                    if (catalogOptions != null) {
  
                        var catalogNames = catalogOptions.split('|');

                        var options = "<form name='catalogSelection'  id='catalogSelection' method='post' action='/Account/SetUserCatalog' ><h2>Select a Catalog</h2>";
                        for (var i = 0; i < catalogNames.length; i++) {
                            options += "<input type='radio' onclick='this.form.submit.disabled=false' name='catalogName' value='" +
                                catalogNames[i] +
                                "'/> " +
                                catalogNames[i] +
                                "<br />";
                        }

                        options += "<br/><input type='button' class='btn btn-primary' name='submit' value='Submit' onclick='postcatalogSelection(this.form.catalogName.value);' disabled='disabled' />";
                        options += "</form><br/><br/>";
                        options += "<script>function postcatalogSelection(catalogName){ jQuery.post('/AJAX/cart.asmx/SetUserCatalogChoice', {catalogName: catalogName}); jQuery('#modalAddToCart').modal('hide'); window.location.reload(1);}</script>";
                        showActionMessageFixedClean(options);
                    }
                }

            }
 
        }

    }


   function clearSessionTimeOutCookies()
    {
        
       $.ajax({
           type: "POST",
           url: "/AJAX/cart.asmx/ClearSessionTimeOutCookies",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (result) {

           },
           error: function (error) {
               console.log(error);

           }

       });
    }


    $(".set-user-catalog-choice").click(function () {
        if ($(this).data("catalogname")) { setUserCatalogChoice($(this).data("catalogname")); }
        event.preventDefault();
    });


    function setUserCatalogChoice(catalogName) {
        
        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/SetUserCatalogChoice",
            data: '{ "catalogName" : ' + catalogName + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

            },
            error: function (error) {
                console.log(error);

            }

        });
    }

    function setProductComparison() {
        if (!$(".unit-compare").length) return;

        setComparisonCheckboxes();

        $(".clear-comparison-control")
            .click(function() {
                removelAllComparisonProducts();
                setComparisonCheckboxes();
            });

        $(".unit-compare input")
            .click(function() {
                var productId = $(this).attr("id").replace("chk-", "");
                if ($(this).prop('checked') == true) {
                    addProductForComparison(productId);
                } else {
                    removeProductForComparison(productId);
                }
                validateComparisonControls();
                toggleComparisonClearControl();
            });

        $(".unit-compare a.compare-trigger")
            .click(function() {
                var checkedIds = getComparisonProductIds();

                if (checkedIds.length < 2) {
                    showActionMessageReload("Please select up to three products.");
                } else {
                    populateComparisonView(checkedIds);
                }
            });
    }

    // #region Product Comparison Helpers

    function removeProductForComparison(productId) {
        var savedProducts = getComparisonProductIds();
            savedProducts = jQuery.grep(savedProducts, function (value) {
                return value != productId;
            });
            Cookies.set(window.ProductComparisonCookieName, JSON.stringify(savedProducts));
    }

    function addProductForComparison(productId) {
        var savedProducts = getComparisonProductIds();
        savedProducts.push(productId);
        Cookies.set(window.ProductComparisonCookieName, JSON.stringify(savedProducts));
    }

    function removelAllComparisonProducts() {
        Cookies.remove(window.ProductComparisonCookieName);
    }

    function getComparisonProductIds() {
        var savedProducts = [];
        var cookieProducts = Cookies.get(window.ProductComparisonCookieName);
        if (cookieProducts) {
            savedProducts = JSON.parse(cookieProducts);
        }
        return savedProducts;
    }

    function validateComparisonControls() {
        var checkedIds = getComparisonProductIds();
        if (checkedIds.length === 3) {
            $('.unit-compare>input:not(:checked)').attr('disabled', 'disabled');
        } else {
            $('.unit-compare>input:not(:checked)').removeAttr('disabled');
        }
    }

    

    function toggleComparisonClearControl() {
        var checkedIds = getComparisonProductIds();
        if (checkedIds.length) {
            $(".clear-comparison-control").show();
        } else {
            $(".clear-comparison-control").hide();
        }
    }

    function setComparisonCheckboxes() {
        var checkedIds = getComparisonProductIds();

        toggleComparisonClearControl();
        

        var comparisonCheckboxes = $('.unit-compare>input');
        for (var i = 0; i < comparisonCheckboxes.length; i++) {
            var comparisonCheckbox = comparisonCheckboxes[i];
            if (!comparisonCheckbox) continue;
            if ($.inArray($(comparisonCheckbox).attr("id").replace("chk-", ""), checkedIds) !== -1) {
                $(comparisonCheckbox).prop("checked", true);
            } else {
                $(comparisonCheckbox).prop("checked", false);
            }
        }

        validateComparisonControls();
    }

    function buildProductView(products) {
        var productHtml = "<div>";
        productHtml +=
        '<section class="content account">' +
        '<div class="">' +
        	'<div class="account-content compare">' +
                '<div class="table-responsive">' +
                    '<table id="table-compare" class="table">' +
                        '<thead>' +
                        '<tr>' +
                            '<th></th>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            productHtml += '<th class="accept product-'+product.ProductId+'"><i class="fa fa-arrows btn btn-primary hidden-xs"></i><i data-productid="' + product.ProductId + '" class="remove-item-compare fa fa-times btn btn-primary"></i></th>';
        }

        productHtml +=
                '</tr>' +
                '</thead>' +
                '<tbody>' +
                '<tr>' +
                    '<td class="title">Product</td>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            productHtml += '<td class="product-' + product.ProductId + '">' +
                '<div class="product-overlay"><div class="product-mask"></div>';
            for (var d = 0; d < 1; d++) {
                var img = product.Images[d];
                if (product.IsNew) {
                    productHtml += '<span class="label label-info">new</span>';
                }
                if (product.IsOnSale) {
                    productHtml += '<span class="label label-danger">sale</span>';
                }
                productHtml += '<img src="' + img.Src + '?w=112&h=150&as=1&bc=ffffff" class="img-responsive product-image-' + (d + 1) + '" alt="" style="display:inline">';
            }
            productHtml += '</div>' +
                '<a href="' + product.Url + '">' + product.Title + '</a>' +
                '</td>';
        }
        // Brand 
        productHtml += '<tr>' +
                            '<td class="title">Brand</td>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            productHtml += '<td class="product-' + product.ProductId + '">' + product.Brand + '</td>';
        }
        productHtml += '</tr>';
        // Description 
        productHtml += '<tr>' +
                            '<td class="title">Description</td>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            productHtml += '<td class="product-' + product.ProductId + '">' + product.Description + '</td>';
        }
        productHtml += '</tr>';
        // Price
        productHtml += '<tr>' +
                            '<td class="title">Price</td>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            if (product.SalePrice !== 0) {
                productHtml += '<td class="product-' + product.ProductId + '"><span class="price"><del><span class="amount">$' + product.Price.toFixed(2) + '</span></del></span></td>';
                productHtml += '<td class="product-' + product.ProductId + '"><span class="price"><ins><span class="amount">$' + product.Price.toFixed(2) + '</span></ins></span></td>';
            } else {
                productHtml += '<td class="product-' + product.ProductId + '"><span class="price"><ins><span class="amount">$' + product.Price.toFixed(2) + '</span></ins></span></td>';
            }
            
        }
        productHtml += '</tr>';
        // Rating
        productHtml += '<tr>' +
                            '<td class="title">Rating</td>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            productHtml += '<td class="product-' + product.ProductId + '">' +
                                '<div class="product-rating">' +
                                    '<i class="fa fa-star' + (product.Rating > 0 ? '' : '-o') + '"></i>' +
                                    '<i class="fa fa-star' + (product.Rating > 1 ? '' : '-o') + '"></i>' +
                                    '<i class="fa fa-star' + (product.Rating > 2 ? '' : '-o') + '"></i>' +
                                    '<i class="fa fa-star' + (product.Rating > 3 ? '' : '-o') + '"></i>' +
                                    '<i class="fa fa-star' + (product.Rating > 4 ? '' : '-o') + '"></i>' +
                                '</div>' +
                '</td>';
        }
        productHtml += '</tr>';

        productHtml += '<tr>' +
                            '<td class="title"></td>';
        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            var variantId = product.VariantBox && product.VariantBox.VariantBoxLines ? product.VariantBox.VariantBoxLines[0].VariantID : 0;
            productHtml += '<td class="product-' + product.ProductId + '"><a data-variantid="' + variantId + '" data-catalog="' + product.CatalogName + '" data-formid="' + product.ProductId + '" class="btn btn-primary variant_btn variant_btn_' + variantId + ' add-to-cart">Add to Cart</a></td>';
        }
        productHtml += '</tr>' +
                        '</tbody>' +
                    '</table>' +
                '</div>' +
            '</div>' +
        '</div>' +
        '</section>';

        productHtml += "</div>";
        return productHtml;
    }

    function setCompareAddToCart() {
        $(".compare a.add-to-cart").click(function () {
            var productId = $(this).attr("data-formid");
            var variantId = $(this).attr("data-variantid");
            var catalogName = $(this).attr("data-catalog");
            var contextItemId = $("#itemid").val();
            addItemToCart(1, productId, catalogName, variantId, contextItemId);
        });
    }


    function populateComparisonView(productIds) {
        var data = "{ productIds:" + JSON.stringify(productIds) + "}";
        $.ajax({
            type: "POST",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "/AJAX/cart.asmx/GetProducts",
            success: function (result) {
                if (!result) return;
                var products = JSON.parse(result.d);
                var productViewHtml = buildProductView(products);
                showActionMessageFixedClean(productViewHtml);
                setCompareAddToCart();
                $('#table-compare').dragtable({
                    dragHandle: '.fa-arrows',
                    dragaccept: '.accept'
                });

                $(".remove-item-compare").click(function () {
                    console.log("removing item");
                    var productId = $(this).attr("data-productid");
                    removeProductForComparison(productId);
                    $(".product-" + productId).remove();
                    var savedProducts = getComparisonProductIds();
                    if (!savedProducts || savedProducts.length===0)  {
                        $("#alertmessage").html("");
                        $("#modalAddToCart").modal("hide");
                        setComparisonCheckboxes();
                    } 
                });
            },
            error: function (error) {
                showActionMessageReload("Error doing product comparison. Please try again later.");
            }
        });
    }

    // #endregion 
    

    function adjustVariantCarousel() {

        if ($(".product-crousel-parent").length > 0) {

            // do something here

            $(".variant_images").hide();
            $(".variant_img_default").show();
            $(".product-crousel-parent").attr("style", "visibility: visible")

            // set first elenment as selected
            if ($(".ProductColor").length > 0) {
                $(".ProductColor option:first").attr("selected", "selected").trigger("change");
            }
        }
    }

    function checkIfCommerceActionsAllowed() {

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/CheckIfCommerceActionsAllowed",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == "") {

                    loadCart();
                    commerceActionAllowed = true;

                } else {

                    commerceActionAllowed = false;
                }
            },
            error: function (error) {
                console.log(error);
            }

        });

    }

    function showDisallowedMessage() {

        showActionMessageFixed("Action DENIED! To this user type.");
    }

    $(".out-of-stock").click(function () {

        showActionMessageFixed("This Product is Currently Out of Stock!");
    });

    $(".add-to-cart").click(function () {

        if ($(this).data("formid") && $(this).data("contextitemid")) { addProductToCart($(this).data("formid"), $(this).data("contextitemid")); }

    });

    function addItemToCart(quantity, productId, catalogName, variantId, contextItemId) {
        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/AddProductToCart",
            data: "{quantity:'" + quantity + "', productId:'" + productId + "', catalogName:'" + catalogName + "', variantId:'" + variantId + "', contextItemId:'" + contextItemId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.d == "") {

                    showActionMessage("Product Added to Cart");
                    loadCart();

                }

            },
            error: function (error) {
                console.log(error);
            }

        });
    }

    function addProductToCart(formId, contextItemId) {

        if (commerceActionAllowed === false) { showDisallowedMessage(); return false; }

        var form = document.getElementById(formId);

        var quantity = form.Quantity.value;

        var productId = form.ProductId.value;
        var catalogName = form.CatalogName.value;
        var variantId = form.VariantId.value;

        addItemToCart(quantity, productId, catalogName, variantId, contextItemId);
    }


    $(".RemoveProduct").click(function () {

        if ($(this).data("externalid")) {
            removeProductFromCart($(this).data("externalid"));
            $(this).parent().parent().hide();
        }

    });


    function removeProductFromCart(externalId) {
        if (commerceActionAllowed === false) { showDisallowedMessage(); return false; }

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/RemoveFromCart",
            data: '{ "externalID" : ' + JSON.stringify(externalId) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                console.log(result.d);

                // modalAddToCart
                showActionMessageReload("Removed from Cart");

                loadCart();

            },
            error: function (error) {
                console.log(error);

            }

        });
    }

    function showCartUpdate(shoppingCart) {

        // Show total update
        $("#cartTotal").text(shoppingCart.Total);

        // Show list of updates
        $("#cart-items-list").empty();

        var cartItems = shoppingCart.CartItems;

        if (cartItems.length > 0) {

            for (var i = 0; i < cartItems.length; i++) {

                $("#cart-items-list").append('<li><div class="row"><div class="col-sm-3"><img src="' + cartItems[i].ImageUrl + '" class="img-responsive" alt=""></div><div class="col-sm-9"><h4><a href="/categories/' + cartItems[i].Category + "/" + cartItems[i].CSProductId + '">' + cartItems[i].ProductName + "</a></h4><p>" + cartItems[i].Quantity + "x - $" + cartItems[i].UnitPrice + '</p><a href="javascript:void(0)" onClick="RemoveProductFromCart(\'' + cartItems[i].ExternalID + '\')" class="remove"><i class="fa fa-times-circle"></i></a></div></div></li>');
            }

            $("#cart-items-list").append('<li><div class="row"><div class="col-sm-6"><a href="/cart" class="btn btn-primary btn-block">View Cart</a></div><div class="col-sm-6"><a href="/checkout" class="btn btn-primary btn-block">Checkout</a></div></div></li>');

        }


    }

    function loadCart() {

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/LoadCart",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var shoppingCart = result.d;
                showCartUpdate(shoppingCart);

            },
            error: function (error) {
                console.log(error);
            }

        });

    }

    $(".update-cart").click(function () {

        submitCartFormData($(this));

    });


    function submitCartFormData(thisObj) {

        if (commerceActionAllowed === false) { showDisallowedMessage(); return false; }

        event.preventDefault();

        var vals = $("#cart-form").values();

        var jsonObject = new Array();

        for (var i = 0; i < vals.quantity.length; i++) {

            var obj = new Object();
            obj.ExternalID = vals.externalID[i];
            obj.Quantity = vals.quantity[i];
            jsonObject.push(obj);
        }


        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/UpdateCartList",
            data: JSON.stringify({ currentCartItems: jsonObject }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {

                showActionMessage("Cart Updated");
                loadCart();
                redirectPage(thisObj.attr("href"));

                return true;
            },
            error: function (error) {
                return false;

            }


        });

    }


    function redirectPage(newPage) {
        window.setTimeout(function () {
            window.location = newPage;
        }, 1760);
    }


    //===== CHECKOUT =====

    function loadCheckoutFormData() {

        // if CheckoutForm exists
        if ($("#checkout-form").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get("checkout_form")) !== "undefined") {

                // Load the data
                // Get data from cookie
                var formValues = Cookies.get("checkout_form");

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#checkout-form").values(formValues);

            }

        }
    }

    function actionMessageFixed(message) {
        // alertmessage
        $("#alertmessage").html(message);
        $("#modalAddToCart").modal("show");
    }


    function showActionMessage(message) {
        // alertmessage
        actionMessageFixed("<h3>" + message + "</h3><br /><br />");
        window.setTimeout(function () {
            $("#modalAddToCart").modal("hide");
        }, 1750);
    }

    function showActionMessageFixed(message) {
        // alertmessage
        actionMessageFixed("<h3>" + message + "</h3><br /><br />");
    }

    function showActionMessageFixedClean(message) {
        // alertmessage
        actionMessageFixed(message);
    }

    function showActionMessageReload(message) {
        // alertmessage
        actionMessageFixed("<h3>" + message + "</h3><br /><br />");
        window.setTimeout(function () {
            $("#modalAddToCart").modal("hide");
            window.location.reload(1);
        }, 2000);
    }


    $(".SubmitCheckout").click(function () {

        submitCheckoutFormData($(this));
        event.preventDefault();
    });

    function submitCheckoutFormData(thisObj) {


        // Validate form, if valid, save data return true or false;

        if ($("#checkout-form").valid()) {

            // save data 
            saveFormData("#checkout-form", "checkout_form");

            var formValues = $("#checkout-form").values();

            console.log(formValues);

            var data = "firstname:'" + formValues.firstname[0] + "',";
            data += "lastname:'" + formValues.lastname[0] + "',";
            data += "email:'" + formValues.email[0] + "',";
            data += "company:'" + formValues.company[0] + "',";
            data += "address:'" + formValues.address[0] + "',";
            data += "addressline1:'" + formValues.addressline1[0] + "',";
            data += "city:'" + formValues.city[0] + "',";
            data += "country:'" + formValues.country[0] + "',";
            data += "fax:'" + formValues.fax[0] + "',";
            data += "phone:'" + formValues.phone[0] + "',";
            data += "zip:'" + formValues.zip[0] + "',";

            if (formValues.firstname2[0] == 0) {
                data += "firstname2:'" + formValues.firstname[0] + "',";
                data += "lastname2:'" + formValues.lastname[0] + "',";
                data += "email2:'" + formValues.email[0] + "',";
                data += "company2:'" + formValues.company[0] + "',";
                data += "address2:'" + formValues.address[0] + "',";
                data += "addressline12:'" + formValues.addressline1[0] + "',";
                data += "city2:'" + formValues.city[0] + "',";
                data += "country2:'" + formValues.country[0] + "',";
                data += "fax2:'" + formValues.fax[0] + "',";
                data += "phone2:'" + formValues.phone[0] + "',";
                data += "zip2:'" + formValues.zip[0] + "',";
            } else {
                data += "firstname2:'" + formValues.firstname2[0] + "',";
                data += "lastname2:'" + formValues.lastname2[0] + "',";
                data += "email2:'" + formValues.email2[0] + "',";
                data += "company2:'" + formValues.company2[0] + "',";
                data += "address2:'" + formValues.address2[0] + "',";
                data += "addressline12:'" + formValues.addressline12[0] + "',";
                data += "city2:'" + formValues.city2[0] + "',";
                data += "country2:'" + formValues.country2[0] + "',";
                data += "fax2:'" + formValues.fax2[0] + "',";
                data += "phone2:'" + formValues.phone2[0] + "',";
                data += "zip2:'" + formValues.zip2[0] + "',";
            }

            data += "billandshipping:'1'";

            console.log(data);

            $.ajax({
                type: "POST",
                url: "/AJAX/cart.asmx/ApplyShippingAndBillingToCart",
                data: "{" + data + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    showActionMessage("Shipping and Billing Applied");
                    redirectPage(thisObj.attr("href"));
                },
                error: function (error) {
                    console.log(error);
                    return false;
                }

            });

            return true;

        } else {

            return false;
        }

        return false;
    }

    //===== CATEGORYPRODUCTS =====

    $("#products_page a").click(function (e) {

        e.preventDefault();

        var currentPage = $(this).attr("val");

        $("#CurrentPage").val(currentPage);

        $("#products_sort").submit();

    });

    $("#prod_page_show").change(function (e) {

        $("#CurrentPage").val("1");

        $("#PageSize").val($(this).val());

        $("#products_sort").submit();

    });

    $("#OrderBy").change(function (e) {

        $("#CurrentPage").val("1");

        $("#products_sort").submit();

    });

    //===== SEARCHPAGING =====

    $("#products_search_page a").click(function (e) {

        e.preventDefault();

        var currentPage = $(this).attr("val");

        $("#pn").val(currentPage);

        $("#products_sort").submit();

    });

    $("#prod_result_size").change(function (e) {

        $("#pn").val("1");

        $("#ps").val($(this).val());

        $("#products_sort").submit();

    });

    $("#s").change(function (e) {

        $("#pn").val("1");

        $("#products_sort").submit();

    });

    //===== FORMDATA MANAGER =====

    /* jQuery.values: get or set all of the name/value pairs from child input controls   
  * @argument data {array} If included, will populate all child controls.
  * @returns element if data was provided, or array of values if not
 */

    $.fn.values = function (data) {
        var els = this.find(":input").get();

        if (arguments.length === 0) {
            // return all data
            data = {};

            $.each(els, function () {
                if (this.name && !this.disabled && (this.checked
                                || /select|textarea/i.test(this.nodeName)
                                || /text|email|hidden|password/i.test(this.type))) {
                    if (data[this.name] == undefined) {
                        data[this.name] = [];
                    }
                    data[this.name].push($(this).val());
                }
            });
            return data;
        } else {
            $.each(els, function () {
                if (this.name && data[this.name]) {
                    var names = data[this.name];
                    var $this = $(this);
                    if (Object.prototype.toString.call(names) !== "[object Array]") {
                        names = [names]; //backwards compat to old version of this code
                    }
                    if (this.type == "checkbox" || this.type == "radio") {
                        var val = $this.val();
                        var found = false;
                        for (var i = 0; i < names.length; i++) {
                            if (names[i] == val) {
                                found = true;
                                break;
                            }
                        }
                        $this.attr("checked", found);
                    } else {
                        $this.val(names[0]);
                    }
                }
            });
            return this;
        }
    };


    function saveFormData(id, name) {

        // If no # before the ID, add it.
        if (id.indexOf("#") === -1) {
            id = "#" + id;
        }

        if ($(id).length) {
            var vals = $(id).values();

            vals = JSON.stringify(vals);

            Cookies.set(name, vals, { expires: 7 });
        }
    }


    //===== PAYMENTS =====
    function loadPaymentFormData() {

        // if CheckoutForm exists
        if ($("#payment-form").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get("payment_form")) !== "undefined") {

                // Load the data
                // Get data from cookie
                var formValues = Cookies.get("payment_form");

                formValues = JSON.parse(formValues);
                console.log(formValues);
                $("#payment-form").values(formValues);

            }

        }
    }



    $(".SubmitPayment").click(function () {

        submitPaymentFormData($(this));
        event.preventDefault();
    });


    function submitPaymentFormData(thisObj) {

        // Validate form, if valid, save data return true or false;

        if ($("#payment-form").valid()) {

            event.preventDefault();


            var radioVal = $(":radio[name='optionsRadios']:checked").val();
            var radioIdx = $(":radio[name='optionsRadios']:checked").index(":radio[name='optionsRadios']");

            // save data 
            saveFormData("#payment-form", "payment_form");

            var formValues = $("#payment-form").values();

            console.log(formValues);

            var data = "paymentExternalID:'" + radioVal + "',";
            data += "nameoncard:'" + formValues.nameoncard[radioIdx] + "',";
            data += "creditcard:'" + formValues.creditcard[radioIdx] + "',";
            data += "expmonth:'" + formValues.expmonth[radioIdx] + "',";
            data += "expyear:'" + formValues.expyear[radioIdx] + "',";
            data += "ccv:'" + formValues.ccv[radioIdx] + "'";

            $.ajax({
                type: "POST",
                url: "/AJAX/cart.asmx/ApplyPaymentMethodToCart",
                data: "{" + data + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    showActionMessage("Payment Method Applied");

                    redirectPage(thisObj.attr("href"));

                },
                error: function (error) {
                    console.log(error);

                }

            });

            return true;

        } else {

            return false;
        }

        return false;
    }

    //===== REVIEW =====


    function loadReviewFormData() {

        // if CheckoutForm exists
        if ($("#review-form").length) {

            var formValues = "";

            if (typeof (Cookies.get("checkout_form")) !== "undefined") {

                formValues = Cookies.get("checkout_form");

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#billingAddress").append("<li><b>" + formValues.firstname[0] + " " + formValues.lastname[0] + "</b><li>");
                $("#billingAddress").append("<li>" + formValues.address[0] + "<li>");
                $("#billingAddress").append("<li>" + formValues.zip[0] + " " + formValues.city[0] + "<li>");
                $("#billingAddress").append("<li>" + formValues.country[0] + "<li>");

                if (formValues.firstname2[0] == 0) {
                    $("#shippingAddress").append("<li><b>" + formValues.firstname[0] + " " + formValues.lastname[0] + "</b><li>");
                    $("#shippingAddress").append("<li>" + formValues.address[0] + "<li>");
                    $("#shippingAddress").append("<li>" + formValues.zip[0] + " " + formValues.city[0] + "<li>");
                    $("#shippingAddress").append("<li>" + formValues.country[0] + "<li>");
                } else {
                    $("#shippingAddress").append("<li><b>" + formValues.firstname2[0] + " " + formValues.lastname2[0] + "</b><li>");
                    $("#shippingAddress").append("<li>" + formValues.address2[0] + "<li>");
                    $("#shippingAddress").append("<li>" + formValues.zip2[0] + " " + formValues.city2[0] + "<li>");
                    $("#shippingAddress").append("<li>" + formValues.country2[0] + "<li>");
                }

                $("#orderDetails").append("<li><b>Email:</b> " + formValues.email[0] + "<li>");
                $("#orderDetails").append("<li><b>Phone:</b> " + formValues.phone[0] + "<li>");

                $("#AdditionalInformation").html(formValues.information[0]);
            }

            if (typeof (Cookies.get("payment_form")) !== "undefined") {

                formValues = Cookies.get("payment_form");

                formValues = JSON.parse(formValues);

                console.log(formValues);
                $("#paymentMethod").append("<li>" + formValues.Payment_Type_Description[0] + "<li>");

            }

            if (typeof (Cookies.get("shipping_form")) !== "undefined") {

                formValues = Cookies.get("shipping_form");

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#shippingMethod").append("<li>" + formValues.optionsRadios[0] + "<li>");

            }

        }
    }


    $(".SubmitReview").click(function () {

        if ($(this).data("contextitemid")) { submitReviewFormData($(this), $(this).data("contextitemid")); }
        event.preventDefault();
    });


    function submitReviewFormData(thisObj, contextItemId) {

        // Validate form, if valid, save data return true or false;

        if ($("#review-form").valid()) {

            event.preventDefault();

            // If checkbox not checked, prevent sublission
            var tandc = $("#checkout_terms_conditions");
            if (tandc[0].checked == true) {

            } else {

                showActionMessageFixed("Please indicate that you accept the Terms and Conditions");

                return false;

            }

            var data = "contextItemId:'" + contextItemId + "',";
            data += "orderTotal:'" + 300 + "'";

            $.ajax({
                type: "POST",
                url: "/AJAX/cart.asmx/SubmitOrder",
                data: "{" + data + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var orderId = result.d;
                    Cookies.set("orderConfirmationID", orderId, { expires: 7 });
                    showActionMessage("Order Submitted!");
                    redirectPage(thisObj.attr("href"));
                },
                error: function (error) {

                    console.log(error);
                }

            });

            return true;

        } else {

            return false;
        }

        return false;
    }

    //===== SHIPPING =====

    function loadShippingFormData() {

        // if CheckoutForm exists
        if ($("#shipping-form").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get("shipping_form")) !== "undefined") {

                // Load the data
                // Get data from cookie
                var formValues = Cookies.get("shipping_form");

                console.log(formValues);

                formValues = JSON.parse(formValues);

                $("#shipping-form").values(formValues);

            }

        }
    }

    $(".SubmitShipping").click(function () {

        submitShippingFormData($(this));
        event.preventDefault();
    });


    function submitShippingFormData(thisObj) {

        // Validate form, if valid, save data return true or false;

        if ($("#shipping-form").valid()) {

            event.preventDefault();

            // save data 
            saveFormData("#shipping-form", "shipping_form");

            var formValues = $("#shipping-form").values();

            console.log(formValues);

            var data = "shippingMethodId:'" + formValues.radio_shipping_2_options[0] + "'";

            $.ajax({
                type: "POST",
                url: "/AJAX/cart.asmx/ApplyShippingMethodToCart",
                data: "{" + data + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    showActionMessage("Shipping Method Applied");

                    redirectPage(thisObj.attr("href"));
                },
                error: function (error) {
                    console.log(error);

                }

            });

            return true;

        } else {

            return false;
        }

        return false;
    }


    //===== PRODUCTS =====
    $(".ProductSize").change(function () {

        if ($(this).data("objid") && $(this).data("num")) { doProductSizeChange($(this).data("objid"), $(this).data("num")); }

    });

    $(".ProductColor").change(function () {

        if ($(this).val() && $(this).data("num")) { doProductColorChange($(this).val(), $(this).data("num")); }

    });

    
    $(".variantcolorrectangle").click(function () {

        $(this).parent().parent().find('li').removeClass("variantcolorselected");

        $(this).parent().addClass("variantcolorselected");

        if ($(this).data("val") && $(this).data("num")) { doProductColorChange($(this).data("val"), $(this).data("num")); }

        event.preventDefault();
    });


    function doProductSizeChange(objId, num) {

        var obj = $(objId);
        var objText = $(objId + " option:selected").text();
        var objValue = $(objId + " option:selected").val();

        var classToHide = ".ProductColor" + num;

        var idToShow = "#" + objText + "ProductColor" + num;

        var ulToShow = "#ul_" + objText + "ProductColor" + num;

        // hide all ProductColor with class   ProductColor + num
        $(classToHide).hide();

        // show only ProductColor with ID ProductColor + num + Obj.Value
        $(idToShow).show();

        // show only ProductColor with ID ProductColor + num + Obj.Value
        $(ulToShow).show();

        // Set first value of item to show as selected
        $(idToShow + " option:selected").prop("selected", false);
        $(idToShow + " option:first").attr("selected", "selected");

        // process color's default value
        doProductColorChange(objValue, num);
    }

    function doProductColorChange(objValue, num) {

        var variantId = "#VariantID" + num;
        var priceId = "#productAmount" + num;
        var vals = objValue.split("|");

        console.log(num);
        console.log(objValue);

        // Set variant
        $(variantId).val(vals[0]);

        // Set price
        $(priceId).text(vals[1]);

        // Product Image Carousel Class
        var imgClass = ".variant_img_" + vals[0];
        $(".variant_images").hide();

        $(imgClass).show();

        // Stock visibility
        var stockClass = ".variants_in_stock_" + vals[0];
        $(".variants_in_stock").hide();
        $(stockClass).show();

        // Variant switching
        var btnClass = ".variant_btn_" + vals[0];
        $(".variant_btn").hide();
        $(btnClass).show();

    }

    //======== THANK YOU MESSAGE ========
    function loadOrderConfirmationData() {

        // if CheckoutForm exists
        if ($("#orderConfirmationID").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get("orderConfirmationID")) !== "undefined") {

                // Load the data
                // Get data from cookie
                var orderConfirmationId = Cookies.get("orderConfirmationID");

                showActionMessage("Your order ID is: " + orderConfirmationId);

                $("#orderConfirmationID").text(orderConfirmationId);
            }

        }

    }

    // =========== Add Promotions Code =============
    $(".ApplyPromo").click(function () {

        var promocode = $("#promocode").val();

        var data = "promoCode:'" + promocode + "'";

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/ApplyPromoCode",
            data: "{" + data + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                console.log(result.d);

                showActionMessageReload("Promocode Applied");

            },
            error: function (error) {
                showActionMessageFixed("ERROR!! Promocode NOT Applied");
                console.log(error);

            }

        });


    });


    // =========== Units Of Measure =============
    if ($("#UOM").length === 1) {
        $(".unit-of-measure .measures").each(function (index, element) {
            $(element).hide();
        });
        $("select#" + $("#UOM").val()).show();
        $("select#UOM").change(function () {
            $(".unit-of-measure .measures").each(function (index, element) {
                $(element).hide();
            });
            $("select#" + $("#UOM").val()).show();
        });
    }

})(window, jQuery);
