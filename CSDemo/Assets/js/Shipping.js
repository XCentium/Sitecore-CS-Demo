
$(document).ready(function () {

    LoadShippingFormData();

});

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

function SubmitShippingFormData() {

    // Validate form, if valid, save data return true or false;

    if ($('#shipping-form').valid()) {

        //Cookies.remove('checkout_form');

        // save data 
        SaveFormData("#shipping-form", 'shipping_form');

        var formValues = $("#shipping-form").values();

        console.log(formValues);

        var data = "shippingMethodId:'" + JSON.stringify(formValues.radio_shipping_2_options[0]) + "'";

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/ApplyShippingMethodToCart",
            data: "{" + data + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                ShowActionMessage('Shipping Method Applied');

            },
            error: function (error) {
                console.log(error)
                alert(error); //alert with HTTP error

            }

        });




       return true;

    } else {

        return false;
    }

    return false;
}

