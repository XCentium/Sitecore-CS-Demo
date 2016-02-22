
$(document).ready(function () {

    LoadCheckoutFormData();

});

function LoadCheckoutFormData() {
    
    // if CheckoutForm exists
    if ($("#checkout-form").length) {
        
        // If checkoutFormCookie exists
        if (typeof (Cookies.get('checkout_form')) !== "undefined") {

            // Load the data
            // Get data from cookie
            var formValues = Cookies.get('checkout_form');

            formValues = JSON.parse(formValues);

           $("#checkout-form").values(formValues);
            
        }

    } 
}

function SubmitCheckoutFormData() {

    // Validate form, if valid, save data return true or false;

    if ($('#checkout-form').valid()) {

        //Cookies.remove('checkout_form');

        // save data 
        SaveFormData("#checkout-form", 'checkout_form');

        return true;

    } else {

        return false;
    }

    return false;
}

