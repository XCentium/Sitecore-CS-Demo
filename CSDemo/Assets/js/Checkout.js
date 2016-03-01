
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

            console.log(formValues);

           $("#checkout-form").values(formValues);
            
        }

    } 
}

function SubmitCheckoutFormData(thisObj) {

    // Validate form, if valid, save data return true or false;

    if ($('#checkout-form').valid()) {

        event.preventDefault();

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
                RedirectPage(thisObj.href);
            },
            error: function (error) {
                console.log(error)
                alert(error); //alert with HTTP error
                return false;
            }

        });


       return true;

    } else {

        return false;
    }

    return false;
}

