
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

