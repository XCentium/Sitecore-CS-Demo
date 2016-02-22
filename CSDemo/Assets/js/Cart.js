
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

            // modalAddToCart
            $('#modalAddToCart').modal('show');
            window.setTimeout(function () {
                $('#modalAddToCart').modal('hide');
            }, 1500);
            
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

            $("#cart-items-list").append('<li><div class="row"><div class="col-sm-3"><img src="' + cartItems[i].ImageUrl + '" class="img-responsive" alt=""></div><div class="col-sm-9"><h4><a href="/productlisting/productdetail?id=' + cartItems[i].ProductID + '">' + cartItems[i].ProductName + '</a></h4><p>' + cartItems[i].Quantity + 'x - $' + cartItems[i].UnitPrice + '</p><a href="javascript:void(0)" onClick="RemoveProductFromCart(' + cartItems[i].ProductID + ')" class="remove"><i class="fa fa-times-circle"></i></a></div></div></li>');
        }

        $("#cart-items-list").append('<li><div class="row"><div class="col-sm-6"><a href="/cart" class="btn btn-primary btn-block">View Cart</a></div><div class="col-sm-6"><a href="/checkout" class="btn btn-primary btn-block">Checkout</a></div></div></li>');
            
        // Ensure session does not timeout if cart has product.
        // If no activity for 45 seconds, reload to keep 
        setTimeout(refresh, 4500);
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

            var ShoppingCart = result.d;

            ShowCartUpdate(ShoppingCart);

        },
        error: function (error) {
            console.log(error)
            alert("Error"); //alert with HTTP error
        }

    });

}

var currtime = new Date().getTime();
$(document.body).bind("mousemove keypress", function (e) {
    currtime = new Date().getTime();
});

function refresh() {
    if (new Date().getTime() - currtime >= 45000)
        window.location.reload(true);
    else
        setTimeout(refresh, 4500);
}

function SubmitCartFormData() {

    var vals = $("#cart-form").values();

    var JSONObject = new Array();

    for (var i = 0; i < vals.q.length; i++) {

        var obj = new Object();
        obj.ProductId = vals.id[i];
        obj.Quantity = vals.q[i];
        JSONObject.push(obj);
    }

    $.ajax({
        type: "POST",
        url: "/AJAX/cart.asmx/UpdateCartList",
        data: JSON.stringify({ currentCartItems: JSONObject }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var ShoppingCart = result.d;
            ShowCartUpdate(ShoppingCart);
            return true;
        },
        error: function (error) {
            console.log(error)
            alert(error); //alert with HTTP error
        }
    });
    return false;
}



//function SubmitCartFormData() {

//    var vals = $("#cart-form").values();

//    var data = "";

//    for (var i = 0; i < vals.q.length; i++) {

//        if (i > 0) { data += ', '; }

//        data += "{\"id\": \"" + vals.id[i] + "\", \"q\": \"" + vals.q[i] + "\"}";
//    }


//    data = "[" + data + "]";

//    // vals = JSON.stringify(vals);

//    //alert(data)

//    //console.log(data)

//    $.ajax({
//        type: "POST",
//        url: "/AJAX/cart.asmx/UpdateCartList",
//        data: data,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            alert('Success!');
//            var ShoppingCart = result.d;
//            ShowCartUpdate(ShoppingCart);

//        },
//        error: function (error) {
//            console.log(error)
//            alert(error); //alert with HTTP error

//        }
//    });
//    return false;
//}



