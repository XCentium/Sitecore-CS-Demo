
(function (window, $, undefined) {

    $(document).ready(function () {

        LoadReviewFormData();

    });


    function LoadReviewFormData() {

        // if CheckoutForm exists
        if ($("#review-form").length) {
      
            if (typeof (Cookies.get('checkout_form')) !== "undefined") {

                var formValues = Cookies.get('checkout_form');

                formValues = JSON.parse(formValues);

                console.log(formValues);

                $("#billingAddress").append('<li><b>' + formValues.firstname[0] + ' ' + formValues.lastname[0]+ '</b><li>');
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

            data = "";

            $.ajax({
                type: "POST",
                url: "/AJAX/cart.asmx/SubmitOrder",
                data: "{contextItemId:'" + contextItemId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var orderID = result.d;
                    Cookies.set("orderConfirmationID", orderID, { expires: 7 });
                    ShowActionMessage('Order Submitted!');
                    RedirectPage(thisObj.href);
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

})(window, jQuery);
