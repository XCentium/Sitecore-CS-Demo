
$(document).ready(function () {

    // LoadCartData();
    LoadCart();

});

function AddProductToCart(formID) {

    var form = document.getElementById(formID);
 
    var Quantity = form.Quantity.value;

    var ProductId = form.ProductId.value;
    var CatalogName = form.CatalogName.value;
    var VariantId = form.VariantId.value;

    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/AddProductToCart",
        data:  "{Quantity:'" + Quantity + "', ProductId:'" + ProductId + "', CatalogName:'" + CatalogName + "', VariantId:'" + VariantId + "'}", 
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            //console.log(result.d)
            //// var ShoppingCart = result.d;

            //alert(result.d);

            // modalAddToCart
            ShowActionMessage("Added to Cart");

            // ShowCartUpdate(ShoppingCart);
            LoadCart();

        },
        error: function (error) {
            console.log(error)
            alert(error); //alert with HTTP error

        }

    });


}

function ShowActionMessage(message) {
    // alertmessage
    $('#alertmessage').html(message);
    $('#modalAddToCart').modal('show');
    window.setTimeout(function () {
        $('#modalAddToCart').modal('hide');
    }, 1750);
}

function RemoveProductFromCart(externalID) {

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
            alert(error); //alert with HTTP error

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

        for (var i = 0; i < cartItems.length; i++){

            $("#cart-items-list").append('<li><div class="row"><div class="col-sm-3"><img src="' + cartItems[i].ImageUrl + '" class="img-responsive" alt=""></div><div class="col-sm-9"><h4><a href="/category/' + cartItems[i].Category + '/' + cartItems[i].CSProductId + '">' + cartItems[i].ProductName + '</a></h4><p>' + cartItems[i].Quantity + 'x - $' + cartItems[i].UnitPrice + '</p><a href="javascript:void(0)" onClick="RemoveProductFromCart(\'' + cartItems[i].ExternalID + '\')" class="remove"><i class="fa fa-times-circle"></i></a></div></div></li>');
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

            //console.log(ShoppingCart);
            ShowCartUpdate(ShoppingCart);

        },
        error: function (error) {
            console.log(error)
            alert("Error"); //alert with HTTP error
        }

    });

}

function SubmitCartFormData() {

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

            // ShowCartUpdate(ShoppingCart);
            ShowActionMessage("Cart Updated");
           LoadCart();
           ReloadPage();
            return true;
        },
        error: function (error) {
            return false;
            console.log(error)
            alert(error); //alert with HTTP error
        },
        //complete: function () {
        //    // modalAddToCart
        //    alert(2);
        //},

    });

    return false;
}

function ReloadPage() {
    window.setTimeout(function () {
        window.location.reload();
    }, 1800);
}


