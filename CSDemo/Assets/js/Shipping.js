
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

            formValues = JSON.parse(formValues);

            $("#shipping-form").values(formValues);
            
        }

    }
}

function SubmitShippingFormData() {

    // Validate form, if valid, save data return true or false;

    if ($('#shipping-form').valid()) {
        alert(2);
        //Cookies.remove('checkout_form');

        // save data 
        SaveFormData("#shipping-form", 'shipping_form');

        return true;

    } else {

        return false;
    }

    return false;
}

