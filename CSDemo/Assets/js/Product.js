function DoProductSizeChange(ObjId, num) {

    var Obj = $(ObjId);
    var objText = $(ObjId + " option:selected").text();
    var objValue = $(ObjId + " option:selected").val();

    var classToHide = ".ProductColor" + num;

    var idToShow = "#" + objText + "ProductColor" + num;


    alert(idToShow);

    // hide all ProductColor with class   ProductColor + num
    $(classToHide).hide();

    // show only ProductColor with ID ProductColor + num + Obj.Value
    $(idToShow).show();

    // Set first value of item to show as selected
    $(idToShow + " option:selected").prop("selected", false);
    $(idToShow + " option:first").attr('selected', 'selected');

    // process color's default value
    DoProductColorChange(objValue, num);
}

function DoProductColorChange(objValue, num) {

    var variantID = "#VariantID" + num;
    var priceID = "#productAmount" + num;
    var vals = objValue.split('|');

    // set variant
    $(variantID).val(vals[0]);

    // set price
    $(priceID).text(vals[1]);

    // set images
    var images = vals[2];
    if (images.length > 5) {
        var imagesArr = images.split(",");

        var firstImage = '' + imagesArr + '';
        
        var outputs = "";
        for (var x = 0; x < imagesArr.length;x++){
            outputs = outputs + '<div class="item"><img src="' + imagesArr[x] + '" class="img-responsive" alt="product image"></div>';

            //if ($("#product-carousel .item").length > 1) {
            //    $("#product-carousel").owlCarousel({
            //        items: 1,
            //        loop: true,
            //        animateOut: 'fadeOut',
            //        animateIn: 'fadeIn'
            //    });
            //}
        }

        // $("#product-carousel").html(outputs);
    }
    // alert(images);
}