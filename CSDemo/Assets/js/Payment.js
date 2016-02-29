
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
            console.log(formValues);
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

        var formValues = $("#payment-form").values();

        console.log(formValues);

        var data = "paymentExternalID:'" + formValues.optionsRadios[0] + "',";
        data += "nameoncard:'" + formValues.nameoncard[0] + "',";
        data += "creditcard:'" + formValues.creditcard[0] + "',";
        data += "expmonth:'" + formValues.expmonth[0] + "',";
        data += "expyear:'" + formValues.expyear[0] + "',";
        data += "ccv:'" + formValues.ccv[0] + "'";
      //  alert(data);

        $.ajax({
            type: "POST",
            url: "/AJAX/cart.asmx/ApplyPaymentMethodToCart",
            data: "{" + data + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                ShowActionMessage('Payment Method Applied');

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

