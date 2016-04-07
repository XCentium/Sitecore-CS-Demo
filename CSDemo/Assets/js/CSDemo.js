
(function (window, $, undefined) {

    //
    var commerceActionAllowed = false;  // This variable will determine if the user can perform commerce actions

    $(document).ready(function () {

        CheckIfCommerceActionsAllowed();
        LoadCheckoutFormData();
        LoadPaymentFormData();
        LoadShippingFormData();
        LoadReviewFormData();
        LoadOrderConfirmationData();
        AdjustVariantCarousel();
    });

    function AdjustVariantCarousel() {

        if ($(".product-crousel-parent").length > 0) {

            // do something here

            $('.variant_images').hide();
            $('.variant_img_default').show();
            $(".product-crousel-parent").attr("style", "visibility: visible")

            // set first elenment as selected
            if ($(".ProductColor").length > 0) {
                $(".ProductColor option:first").attr('selected', 'selected').trigger('change');
            }
        }
    }

    function CheckIfCommerceActionsAllowed() {

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/CheckIfCommerceActionsAllowed",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == "") {

                    LoadCart();
                    commerceActionAllowed = true;

                } else {

                    commerceActionAllowed = false;
                }
            },
            error: function (error) {
                console.log(error)
            }

        });

    }

    function ShowDisallowedMessage() {

        ShowActionMessage("Action DENIED! To this user type.");
    }

    $('.out-of-stock').click(function () {

        ShowActionMessage("This Product is Currently Out of Stock!");
    });

    $('.add-to-cart').click(function () {

        if ($(this).data('formid') && $(this).data('contextitemid')) { AddProductToCart($(this).data('formid'), $(this).data('contextitemid')); }

    });


    function AddProductToCart(formID, contextItemId) {

        if (commerceActionAllowed === false) { ShowDisallowedMessage(); return false; }

        var form = document.getElementById(formID);

        var Quantity = form.Quantity.value;

        var ProductId = form.ProductId.value;
        var CatalogName = form.CatalogName.value;
        var VariantId = form.VariantId.value;

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/AddProductToCart",
            data: "{Quantity:'" + Quantity + "', ProductId:'" + ProductId + "', CatalogName:'" + CatalogName + "', VariantId:'" + VariantId + "', contextItemId:'" + contextItemId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.d == "") {

                    ShowActionMessage("Added to Cart");
                    LoadCart();

                } else {
                    ShowActionMessage(result.d);
                }

            },
            error: function (error) {
                console.log(error)
            }

        });
    }


    $('.RemoveProduct').click(function () {

        if ($(this).data('externalid')) {
            RemoveProductFromCart($(this).data('externalid'));
            $(this).parent().parent().hide();
        }

    });


    function RemoveProductFromCart(externalID) {
        if (commerceActionAllowed === false) { ShowDisallowedMessage(); return false; }

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/RemoveFromCart",
            data: '{ "externalID" : ' + JSON.stringify(externalID) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                console.log(result.d)
                var ShoppingCart = result.d;

                // modalAddToCart
                ShowActionMessage("Removed from Cart");

                // ShowCartUpdate(ShoppingCart);
                LoadCart();

            },
            error: function (error) {
                console.log(error)

            },

        });
    }

    function ShowCartUpdate(ShoppingCart) {

        // Show total update
        $("#cartTotal").text(ShoppingCart.Total);


        // Show list of updates
        $("#cart-items-list").empty();

        var cartItems = ShoppingCart.CartItems;

        if (cartItems.length > 0) {

            for (var i = 0; i < cartItems.length; i++) {

                $("#cart-items-list").append('<li><div class="row"><div class="col-sm-3"><img src="' + cartItems[i].ImageUrl + '" class="img-responsive" alt=""></div><div class="col-sm-9"><h4><a href="/categories/' + cartItems[i].Category + '/' + cartItems[i].CSProductId + '">' + cartItems[i].ProductName + '</a></h4><p>' + cartItems[i].Quantity + 'x - $' + cartItems[i].UnitPrice + '</p><a href="javascript:void(0)" onClick="RemoveProductFromCart(\'' + cartItems[i].ExternalID + '\')" class="remove"><i class="fa fa-times-circle"></i></a></div></div></li>');
            }

            $("#cart-items-list").append('<li><div class="row"><div class="col-sm-6"><a href="/cart" class="btn btn-primary btn-block">View Cart</a></div><div class="col-sm-6"><a href="/checkout" class="btn btn-primary btn-block">Checkout</a></div></div></li>');

        }


    }

    function LoadCart() {

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/LoadCart",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ShoppingCart = result.d;
                ShowCartUpdate(ShoppingCart);

            },
            error: function (error) {
                console.log(error)
            }

        });

    }

    $('.update-cart').click(function () {

        SubmitCartFormData($(this));

    });


    function SubmitCartFormData(thisObj) {

        if (commerceActionAllowed === false) { ShowDisallowedMessage(); return false; }

        event.preventDefault();

        var vals = $("#cart-form").values();

        var JSONObject = new Array();

        for (var i = 0; i < vals.quantity.length; i++) {

            var obj = new Object();
            obj.ExternalID = vals.externalID[i];
            obj.Quantity = vals.quantity[i];
            JSONObject.push(obj);
        }


        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/UpdateCartList",
            data: JSON.stringify({ currentCartItems: JSONObject }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {

                ShowActionMessage("Cart Updated");
                LoadCart();
                RedirectPage(thisObj.attr('href'));
                // ReloadPage();
                return true;
            },
            error: function (error) {
                return false;
                console.log(error)
            },


        });

    }

    function ReloadPage() {
        window.setTimeout(function () {
            window.location.reload();
        }, 1800);
    }

    function RedirectPage(newPage) {
        window.setTimeout(function () {
            window.location = newPage;
        }, 1760);
    }


    //===== CHECKOUT =====

    function LoadCheckoutFormData() {

        // if CheckoutForm exists
        if ($("#checkout-form").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get('checkout_form')) !== "undefined") {

                // Load the data
                // Get data from cookie
                var formValues = Cookies.get('checkout_form');

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#checkout-form").values(formValues);

            }

        }
    }

    function ShowActionMessage(message) {
        // alertmessage
        $('#alertmessage').html(message);
        $('#modalAddToCart').modal('show');
        window.setTimeout(function () {
            $('#modalAddToCart').modal('hide');
        }, 1750);
    }



    $('.SubmitCheckout').click(function () {

        SubmitCheckoutFormData($(this));
        event.preventDefault();
    });

    function SubmitCheckoutFormData(thisObj) {


        // Validate form, if valid, save data return true or false;

        if ($('#checkout-form').valid()) {

            // save data 
            SaveFormData("#checkout-form", 'checkout_form');

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

                    ShowActionMessage('Shipping and Billing Applied');
                    RedirectPage(thisObj.attr('href'));
                },
                error: function (error) {
                    console.log(error)
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
        var els = this.find(':input').get();

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
                    if (Object.prototype.toString.call(names) !== '[object Array]') {
                        names = [names]; //backwards compat to old version of this code
                    }
                    if (this.type == 'checkbox' || this.type == 'radio') {
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


    function SaveFormData(id, name) {

        // If no # before the ID, add it.
        if (id.indexOf('#') === -1) {
            id = "#" + id;
        }

        if ($(id).length) {
            var vals = $(id).values();

            vals = JSON.stringify(vals);

            Cookies.set(name, vals, { expires: 7 });
        }
    }


    //===== PAYMENTS =====


    function LoadPaymentFormData() {

        // if CheckoutForm exists
        if ($("#payment-form").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get('payment_form')) !== "undefined") {

                // Load the data
                // Get data from cookie
                var formValues = Cookies.get('payment_form');

                formValues = JSON.parse(formValues);
                console.log(formValues);
                $("#payment-form").values(formValues);

            }

        }
    }



    $('.SubmitPayment').click(function () {

        SubmitPaymentFormData($(this));
        event.preventDefault();
    });


    function SubmitPaymentFormData(thisObj) {

        // Validate form, if valid, save data return true or false;

        if ($('#payment-form').valid()) {

            event.preventDefault();


            var radioVal = $(":radio[name='optionsRadios']:checked").val();
            var radioIdx = $(":radio[name='optionsRadios']:checked").index(":radio[name='optionsRadios']");

            // save data 
            SaveFormData("#payment-form", 'payment_form');

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

                    ShowActionMessage('Payment Method Applied');

                    RedirectPage(thisObj.attr('href'));

                },
                error: function (error) {
                    console.log(error)

                }

            });

            return true;

        } else {

            return false;
        }

        return false;
    }

    //===== REVIEW =====


    function LoadReviewFormData() {

        // if CheckoutForm exists
        if ($("#review-form").length) {

            if (typeof (Cookies.get('checkout_form')) !== "undefined") {

                var formValues = Cookies.get('checkout_form');

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#billingAddress").append('<li><b>' + formValues.firstname[0] + ' ' + formValues.lastname[0] + '</b><li>');
                $("#billingAddress").append('<li>' + formValues.address[0] + '<li>');
                $("#billingAddress").append('<li>' + formValues.zip[0] + ' ' + formValues.city[0] + '<li>');
                $("#billingAddress").append('<li>' + formValues.country[0] + '<li>');

                if (formValues.firstname2[0] == 0) {
                    $("#shippingAddress").append('<li><b>' + formValues.firstname[0] + ' ' + formValues.lastname[0] + '</b><li>');
                    $("#shippingAddress").append('<li>' + formValues.address[0] + '<li>');
                    $("#shippingAddress").append('<li>' + formValues.zip[0] + ' ' + formValues.city[0] + '<li>');
                    $("#shippingAddress").append('<li>' + formValues.country[0] + '<li>');
                } else {
                    $("#shippingAddress").append('<li><b>' + formValues.firstname2[0] + ' ' + formValues.lastname2[0] + '</b><li>');
                    $("#shippingAddress").append('<li>' + formValues.address2[0] + '<li>');
                    $("#shippingAddress").append('<li>' + formValues.zip2[0] + ' ' + formValues.city2[0] + '<li>');
                    $("#shippingAddress").append('<li>' + formValues.country2[0] + '<li>');
                }

                $("#orderDetails").append('<li><b>Email:</b> ' + formValues.email[0] + '<li>');
                $("#orderDetails").append('<li><b>Phone:</b> ' + formValues.phone[0] + '<li>');

                $("#AdditionalInformation").html(formValues.information[0]);
            }

            if (typeof (Cookies.get('payment_form')) !== "undefined") {

                var formValues = Cookies.get('payment_form');

                formValues = JSON.parse(formValues);

                console.log(formValues);
                $("#paymentMethod").append('<li>' + formValues.Payment_Type_Description[0] + '<li>');

            }

            if (typeof (Cookies.get('shipping_form')) !== "undefined") {

                var formValues = Cookies.get('shipping_form');

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#shippingMethod").append('<li>' + formValues.optionsRadios[0] + '<li>');

            }

        }
    }


    $('.SubmitReview').click(function () {

        if ($(this).data('contextitemid')) { SubmitReviewFormData($(this), $(this).data('contextitemid')); }
        event.preventDefault();
    });


    function SubmitReviewFormData(thisObj, contextItemId) {

        // Validate form, if valid, save data return true or false;

        if ($('#review-form').valid()) {

            event.preventDefault();

            // If checkbox not checked, prevent sublission
            var tandc = $("#checkout_terms_conditions");
            if (tandc[0].checked == true) {

            } else {

                ShowActionMessage("Please indicate that you accept the Terms and Conditions");

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

                    var orderID = result.d;
                    Cookies.set("orderConfirmationID", orderID, { expires: 7 });
                    ShowActionMessage('Order Submitted!');
                    RedirectPage(thisObj.attr('href'));
                },
                error: function (error) {

                    console.log(error)

                }

            });

            // save data 
            // SaveFormData("#review-form", 'review_form');

            return true;

        } else {

            return false;
        }

        return false;
    }

    //===== SHIPPING =====

    function LoadShippingFormData() {

        // if CheckoutForm exists
        if ($("#shipping-form").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get('shipping_form')) !== "undefined") {

                // Load the data
                // Get data from cookie
                var formValues = Cookies.get('shipping_form');

                console.log(formValues);

                formValues = JSON.parse(formValues);

                $("#shipping-form").values(formValues);

            }

        }
    }

    $('.SubmitShipping').click(function () {

        SubmitShippingFormData($(this));
        event.preventDefault();
    });


    function SubmitShippingFormData(thisObj) {

        // Validate form, if valid, save data return true or false;

        if ($('#shipping-form').valid()) {

            event.preventDefault();

            // save data 
            SaveFormData("#shipping-form", 'shipping_form');

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

                    ShowActionMessage('Shipping Method Applied');

                    RedirectPage(thisObj.attr('href'));
                },
                error: function (error) {
                    console.log(error)

                }

            });




            return true;

        } else {

            return false;
        }

        return false;
    }


    //===== PRODUCTS =====

    $('.ProductSize').change(function () {

        if ($(this).data('objid') && $(this).data('num')) { DoProductSizeChange($(this).data('objid'), $(this).data('num')); }

    });

    $('.ProductColor').change(function () {

        if ($(this).val() && $(this).data('num')) { DoProductColorChange($(this).val(), $(this).data('num')); }

    });


    function DoProductSizeChange(ObjId, num) {

        var Obj = $(ObjId);
        var objText = $(ObjId + " option:selected").text();
        var objValue = $(ObjId + " option:selected").val();

        var classToHide = ".ProductColor" + num;

        var idToShow = "#" + objText + "ProductColor" + num;

        // hide all ProductColor with class   ProductColor + num
        $(classToHide).hide();

        // show only ProductColor with ID ProductColor + num + Obj.Value
        $(idToShow).show();

        // Set first value of item to show as selected
        $(idToShow + " option:selected").prop("selected", false);
        $(idToShow + " option:first").attr('selected', 'selected');

        // process color's default value
        DoProductColorChange(objValue, num);
    }

    function DoProductColorChange(objValue, num) {

        var variantID = "#VariantID" + num;
        var priceID = "#productAmount" + num;
        var vals = objValue.split('|');

        console.log(num);
        console.log(objValue);

        // set variant
        $(variantID).val(vals[0]);

        // set price
        $(priceID).text(vals[1]);

        // Image Carousel Class
        var imgClass = '.variant_img_' + vals[0];
        $('.variant_images').hide();
        // $('.product-carousel-wrapper').hide();
        // $(imgClass).css({position: 'static'});
        $(imgClass).show();

        // Stock
        var stockClass = '.variants_in_stock_' + vals[0];
        $('.variants_in_stock').hide();
        $(stockClass).show();

        // variant_btn
        var btnClass = '.variant_btn_' + vals[0];
        $('.variant_btn').hide();
        $(btnClass).show();

    }

    //======== THANKYOU ========

    function LoadOrderConfirmationData() {

        // if CheckoutForm exists
        if ($("#orderConfirmationID").length) {

            // If checkoutFormCookie exists
            if (typeof (Cookies.get('orderConfirmationID')) !== "undefined") {

                // Load the data
                // Get data from cookie
                var orderConfirmationID = Cookies.get('orderConfirmationID');

                ShowActionMessage('Your order ID is: ' + orderConfirmationID);

                $('#orderConfirmationID').text(orderConfirmationID);
            }

        }

    }


    $('.ApplyPromo').click(function () {

        var promocode = $('#promocode').val();

        var data = "promoCode:'" + promocode + "'";

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/ApplyPromoCode",
            data: "{" + data + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                console.log(result.d)
                var ShoppingCart = result.d;

                // modalAddToCart
                ShowActionMessage("Promocode Applied");

            },
            error: function (error) {
                ShowActionMessage("ERROR!! Promocode NOT Applied");
                console.log(error)

            },

        });


    });


})(window, jQuery);
