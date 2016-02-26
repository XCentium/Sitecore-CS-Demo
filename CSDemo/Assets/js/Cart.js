
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
    }, 1500);
}


function AddToCart(ProductID) {

    alert('Error');

    //$.ajax({
    //    type: "POST",
    //    url: "/AJAX/cart.asmx/AddToCart",
    //    data: '{ "productID" : ' + JSON.stringify(ProductID) + '}',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (result) {

    //        console.log(result.d)
    //        var ShoppingCart = result.d;

    //        // modalAddToCart
    //        $('#modalAddToCart').modal('show');
    //        window.setTimeout(function () {
    //            $('#modalAddToCart').modal('hide');
    //        }, 1500);
            
    //        ShowCartUpdate(ShoppingCart);

    //    },
    //    error: function (error) {
    //        console.log(error)
    //        alert(error); //alert with HTTP error

    //    }

    //});
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

        }

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

            $("#cart-items-list").append('<li><div class="row"><div class="col-sm-3"><img src="' + cartItems[i].ImageUrl + '" class="img-responsive" alt=""></div><div class="col-sm-9"><h4><a href="/productlisting/productdetail?id=' + cartItems[i].ProductID + '">' + cartItems[i].ProductName + '</a></h4><p>' + cartItems[i].Quantity + 'x - $' + cartItems[i].UnitPrice + '</p><a href="javascript:void(0)" onClick="RemoveProductFromCart(' + JSON.stringify(cartItems[i].ExternalID) + ')" class="remove"><i class="fa fa-times-circle"></i></a></div></div></li>');
        }

        $("#cart-items-list").append('<li><div class="row"><div class="col-sm-6"><a href="/cart" class="btn btn-primary btn-block">View Cart</a></div><div class="col-sm-6"><a href="/checkout" class="btn btn-primary btn-block">Checkout</a></div></div></li>');
            
        // Ensure session does not timeout if cart has product.
        // If no activity for 45 seconds, reload to keep 
        setTimeout(refresh, 4500);
    }


}

function LoadCart() {

    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/LoadCart",
        data: '{}',
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


function refresh() {
    //if (new Date().getTime() - currtime >= 45000)
    //    window.location.reload(true);
    //else
    //    setTimeout(refresh, 4500);
}

function SubmitCartFormData() {

    var vals = $("#cart-form").values();

    var JSONObject = new Array();

    for (var i = 0; i < vals.quantity.length; i++) {

        var obj = new Object();
        obj.ExternalID = JSON.stringify(vals.externalID[i]);
        obj.Quantity = vals.quantity[i];
        JSONObject.push(obj);
    }

    console.log(JSONObject);


    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/UpdateCartList",
        data: JSON.stringify({ currentCartItems: JSONObject }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {


            // modalAddToCart
            ShowActionMessage("Cart Updated");

            // ShowCartUpdate(ShoppingCart);
            LoadCart();

            return true;
        },
        error: function (error) {
            console.log(error)
            alert(error); //alert with HTTP error
        }
    });
    return false;
}




