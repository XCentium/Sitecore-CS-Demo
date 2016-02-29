/*----------------------------------------------------
 * Copyright 2014 Pixelized Studio
 * http://www.pixelized.cz
 * Color Switcher
 * color-switcher.js
 *
 * Version 3.0
 * Released: 12.12.2014
 * http://creativecommons.org/licenses/by/4.0/
----------------------------------------------------*/

/* --------------------------------------
 * Updated by C.Castle of XCentium to
 * include the bits with localStorage
 * to carry the theme color over from
 * page to page, and future visits.
-------------------------------------- */

$(document).ready(function () {

    // Set the theme color at page load to match what is in local storage
    if (localStorage.themeColor != undefined) {
        $("#main-color").attr("href", "assets/css/color/" + localStorage.themeColor + ".css");
    }

	var CSduration = 500;
	$('#color-switcher > ul > li').tooltip();
	
	$('#toggle-switcher').click(function(){
		if($(this).hasClass('opened')) {
			$(this).removeClass('opened');
			$(this).find('i').removeClass('fa-times');
			$(this).find('i').addClass('fa-gear');
			$('#color-switcher').animate({'left':'-214px'},CSduration);
		}
		else {
			$(this).addClass('opened')
			$(this).find('i').removeClass('fa-gear');
			$(this).find('i').addClass('fa-times');
			$('#color-switcher').animate({'left':'0'},CSduration);
		}
	});
	
	$('#color-switcher > ul > li').click(function () {
	    var color = $(this).attr("id");
	    localStorage.themeColor = color;

		$("#main-color").attr("href","assets/css/color/" + color + ".css");
	});
	
	$('#page-boxed-toggle').click(function() {
        $('#page-wrapper').toggleClass("page-boxed");
		$('body').toggleClass("page-boxed");
		$(this).toggleClass("active");
    });
});




