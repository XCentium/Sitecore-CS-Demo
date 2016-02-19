
$(document).ready(function () {

    LoadCartData();

});


function AddToCart(ProductID) {

    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/AddToCart",
        data: '{ "productID" : ' + JSON.stringify(ProductID) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result.d)
            var ShoppingCart = result.d;

            
            ShowCartUpdate(ShoppingCart);

        },
        error: function (error) {
            console.log(error)
            alert(error); //alert with HTTP error

        }

    });
}

function RemoveProductFromCart(ProductID) {

    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/RemoveFromCart",
        data: '{ "productID" : ' + JSON.stringify(ProductID) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result.d)
            var ShoppingCart = result.d;


            ShowCartUpdate(ShoppingCart);

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
            console.log(i)
            $("#cart-items-list").append('<li><div class="row"><div class="col-sm-3"><img src="' + cartItems[i].ImageUrl + '" class="img-responsive" alt=""></div><div class="col-sm-9"><h4><a href="/productlisting/productdetail?id=' + cartItems[i].ProductID + '">' + cartItems[i].ProductName + '</a></h4><p>' + cartItems[i].Quantity + 'x - $' + cartItems[i].UnitPrice + '</p><a href="javascript:void(0)" onClick="RemoveProductFromCart(' + cartItems[i].ProductID + ')" class="remove"><i class="fa fa-times-circle"></i></a></div></div></li>');
        }

        $("#cart-items-list").append('<li><div class="row"><div class="col-sm-6"><a href="/cart" class="btn btn-primary btn-block">View Cart</a></div><div class="col-sm-6"><a href="/checkout" class="btn btn-primary btn-block">Checkout</a></div></div></li>');
    }


}

function LoadCartData() {

    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/LoadCart",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result.d)
            var ShoppingCart = result.d;


            ShowCartUpdate(ShoppingCart);

        },
        error: function (error) {
            console.log(error)
            alert("Error"); //alert with HTTP error

        }

    });


}




