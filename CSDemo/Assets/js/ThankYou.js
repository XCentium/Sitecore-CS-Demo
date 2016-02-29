$(document).ready(function () {

    LoadOrderConfirmationData();

});

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