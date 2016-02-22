
$(document).ready(function () {

    LoadPaymentFormData();

});

function LoadPaymentFormData() {

    // if CheckoutForm exists
    if ($("#payment-form").length) {

        // If checkoutFormCookie exists
        if (typeof (Cookies.get('payment_form')) !== "undefined") {

            // Load the data
            // Get data from cookie
            var formValues = Cookies.get('payment_form');

            formValues = JSON.parse(formValues);

            $("#payment-form").values(formValues);

        }

    }
}

function SubmitPaymentFormData() {

    // Validate form, if valid, save data return true or false;

    if ($('#payment-form').valid()) {

        //Cookies.remove('checkout_form');

        // save data 
        SaveFormData("#payment-form", 'payment_form');

        return true;

    } else {

        return false;
    }

    return false;
}

