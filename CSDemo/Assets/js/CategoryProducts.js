
$("#products_page a").click(function (e) {

    e.preventDefault();

    var currentPage = $(this).attr("val");

    $("#CurrentPage").val(currentPage);

    $("#products_sort").submit();

});

$("#prod_page_show").change(function (e) {

    $("#CurrentPage").val("1");

    $("#PageSize").val($(this).val());

    $("#products_sort").submit();

});

$("#OrderBy").change(function (e) {

    $("#CurrentPage").val("1");

    $("#products_sort").submit();

});


//var curr_prod_page = 1;
//var prod_page_max = 1000;
//var prod_page_hash = "CurrentPage=1";
//var prod_page_size = "";
//var prod_page_sort = "";
//var prod_category = "";
//var storedPage = [];
//storedPage["1"] = "cool";
//storedPage["2"] = "cat";
//storedPage["3"] = "cam";

//var storedPagesPointer = [];
//storedPagesPointer["1"] = "1";
//storedPagesPointer["2"] = "2";
//storedPagesPointer["3"] = "3";

//var storedPagesRebuild = false;

//window.addEventListener("hashchange", RespondToHash, false);

//$(document).ready(function () {
    
//    Load_Product_List();
 

//});

//$("#products_page a").click(function (e) {

//    e.preventDefault();

//    var clicked_page_nav = $(this).attr("val");

//    Set_curr_prod_page(clicked_page_nav);

//    $(this).closest('ul').find('li').removeClass('active');

//    $(this).parent().addClass('active');

//    buildHashUrl();

//});

//$("#prod_page_show").change(function (e) {

//    storedPagesRebuild = true;

//    buildHashUrl();

//});

//$("#prod_page_sort").change(function (e) {

//    storedPagesRebuild = true;

//    buildHashUrl();

//});

//function buildHashUrl() {

//    var products_per_pageformValues = $("#products_per_page").values();
//    var products_sortformValues = $("#products_sort").values();

//    var hash = "currentPage=" + curr_prod_page;

//    if (products_sortformValues.prod_category[0]) {
//        hash = hash + "|" + "Category=" + products_sortformValues.prod_category[0];
//        //        prod_page_sort + products_sortformValues.prod_category[0];
//    }

//    if (products_per_pageformValues.prod_page_show[0]) {       
//        hash = hash + "|" + "PageSize=" + products_per_pageformValues.prod_page_show[0];
////        prod_page_size = products_per_pageformValues.prod_page_show[0];
//    }

//    if (products_sortformValues.prod_page_sort[0]) {
//        hash = hash + "|" + "OrderBy=" + products_sortformValues.prod_page_sort[0];
////        prod_page_sort = products_sortformValues.prod_page_sort[0];
//    }

//    console.log(products_per_pageformValues);
//    console.log(products_sortformValues);

//    window.location.hash = hash;

//}

//// Pagination script
//function Set_curr_prod_page(data) {

//    if (+data === parseInt(data)) {
//        curr_prod_page = data;
        
//    } else {
//        if (data == "left") {
//            if (curr_prod_page > 1) { curr_prod_page = parseInt(curr_prod_page) - 1; }
//        }
//        if (data == "right") {
//            if (curr_prod_page < prod_page_max) { curr_prod_page = parseInt(curr_prod_page) + 1; }
//        }
//    }

//}

//function RespondToHash() {

//    if (window.location.hash && $("#products_per_page").length) {

//        prod_page_hash = window.location.hash.substring(1);

//        curr_prod_page = 1;
//        prod_page_size = "";
//        prod_page_sort = "";
//        prod_category = "";

//        if (prod_page_hash) {

//            var hash = prod_page_hash;
//            hash = hash.replaceAll("|", "'|");
//            hash = hash.replaceAll("=", "='");
//            hash = hash.replaceAll("CurrentPage", "curr_prod_page");
//            hash = hash.replaceAll("PageSize", "prod_page_size");
//            hash = hash.replaceAll("OrderBy", "prod_page_sort");
//            hash = hash.replaceAll("Category", "prod_category");
//            hash = hash + "'";
//            //alert(hash)
//            hash_array = hash.split('|');
//            //alert(555)
//            for (i = 0; i < hash_array.length; i++) {
//                alert(hash_array[i])
//                eval(hash_array[i]);
//            }
//        }
        
//        Load_Product_List();

//        //// hash found show executive team
//        //if (hash == 'our-team') {
//        //    $('.executives-popup').show('slow');
//        //    $('html, body').animate({
//        //        scrollTop: $('#executive-spotlight').offset().top - 250
//        //    }, 800, function () {
//        //        // Add hash (#) to URL when done scrolling (default click behavior)
//        //        window.location.hash = hash;
//        //    });
//        //}
//    }
//}

//function ss(mess, val) {
//    val = mess + " = " + val;
//    alert(val);
//}

//function Load_Product_List() {

//    ss("curr_prod_page", curr_prod_page)


//    if ($("#products_per_page").length) {

//        if (storedPagesRebuild) {
//            RebuildStorage(0);
//        }

//        var checkStore = $.inArray(curr_prod_page.toString(), storedPagesPointer);

//        ss("checkStore", checkStore)

//        // check if the page exist in storage
//        if (checkStore > -1) {

//            //alert("Serving:" + checkStore);
//            ServePage(storedPage[checkStore]);

//            if (parseInt(curr_prod_page) > 1) {
//                // They are looking at prev data, need to rebuild another prev
//                if (parseInt(checkStore) == 1) {

//                    storedPagesPointer["2"] = storedPagesPointer["1"];
//                    storedPage["2"] = storedPage["1"];

//                    storedPagesPointer["3"] = storedPagesPointer["2"];
//                    storedPage["3"] = storedPage["2"];
//                    var thePage = parseInt(curr_prod_page) - 1;
//                    storedPagesPointer["1"] = thePage.toString();
//                    storedPage["1"] = Load_Product_List_page(parseInt(curr_prod_page) - 1);


//                }

//                // They are looking at next page data, need to rebuild another next
//                if (parseInt(checkStore) == 3) {

//                    storedPagesPointer["1"] = storedPagesPointer["2"];
//                    storedPage["1"] = storedPage["2"];

//                    storedPagesPointer["2"] = storedPagesPointer["3"];
//                    storedPage["2"] = storedPage["3"];

//                    var thePage = parseInt(curr_prod_page) + 1;
//                    storedPagesPointer["3"] = thePage.toString();
//                    storedPage["3"] = Load_Product_List_page(parseInt(curr_prod_page) + 1);
//                }

//            }



//        } else {
//            // page does not exist in storage rebuild page to storage 2, serve it and build storage 1 and 3
//            var data = Load_Product_List_page(curr_prod_page);
//            ServePage(data);

//            storedPagesPointer["2"] = curr_prod_page.toString();
//            storedPage["2"] = data;

//            RebuildStorage(2);
//        }
//    }

//}

//function ServePage(HTML) {

//    $("#products").html(HTML);    
//}

//function Load_Product_List_page(pageNumber){

//    //    alert("NOT IMPLEMENTED Load_Product_List_page");

//    var data = "CurrentPage:'" + pageNumber + "',";
//    data += "pageSize:'" + prod_page_size + "',";
//    data += "orderBy:'" + prod_page_sort + "',";
////    data += "category:'" + category + "',";

//    alert(data);

//    //$.ajax({
//    //    type: "POST",
//    //    url: "/AJAX/cart.asmx/ApplyShippingAndBillingToCart",
//    //    data: "{" + data + "}",
//    //    contentType: "application/json; charset=utf-8",
//    //    dataType: "json",
//    //    success: function (result) {


//    //        ShowActionMessage('Shipping and Billing Applied');
//    //        RedirectPage(thisObj.href);
//    //    },
//    //    error: function (error) {
//    //        console.log(error)
//    //        alert(error); //alert with HTTP error
//    //        return false;
//    //    }

//    //});

//    return "<h1>DATA FOR PAGE " + pageNumber + "</h1>";
//}

//function RebuildStorage(excludeStorage){

//    var thePage = parseInt(curr_prod_page);

//    if (parseInt(curr_prod_page) > 1) {

//        if (parseInt(excludeStorage) != 1) {
//            thePage = parseInt(curr_prod_page) - 1;
//            storedPagesPointer["1"] = thePage.toString();
//            storedPage["1"] = Load_Product_List_page(parseInt(curr_prod_page) - 1);
//        }

//    }

//    if (parseInt(excludeStorage) != 2) {
//        thePage = parseInt(curr_prod_page);
//        storedPagesPointer["2"] = thePage.toString();
//        storedPage["2"] = Load_Product_List_page(parseInt(curr_prod_page));
//    }

//    if (parseInt(excludeStorage) != 3) {
//        thePage = parseInt(curr_prod_page) + 1;
//        storedPagesPointer["3"] = thePage.toString();
//        storedPage["3"] = Load_Product_List_page(parseInt(curr_prod_page) + 1);
//    }

//    storedPagesRebuild = false;
//}


//String.prototype.replaceAll = function (str1, str2, ignore) {
//    return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
//}