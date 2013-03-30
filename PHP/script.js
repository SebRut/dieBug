// JavaScript Document

var iwidth = 0;
var iheight = 0;

var swidth;
var sheight;




$(document).ready(function() { 
	$.blockUI({
		css: {
			border: 'none', 
			padding: '15px', 
			fontFamily: 'Lato',
			fontSize: '30px',
			backgroundColor: '#000', 
			'-webkit-border-radius': '10px', 
			'-moz-border-radius': '10px', 
			opacity: .5, 
			color: '#fff' 
		},
		message: 'Laden...'
	});
	$('#arrow').mouseenter(function(e) {
		$('#arrow').stop().animate({right: '33px'},300,'easeOutExpo');
		$('#sidebar').stop().animate({width: '33px'},300,'easeOutExpo');
		$('#sidebarblocker').stop().animate({opacity: '0.5'},300,'easeOutExpo');
	});
	
	$('#sidebar').mouseenter(function(e) {
		$('#arrow').stop().animate({right: '33px'},300,'easeOutExpo');
		$('#sidebar').stop().animate({width: '33px'},300,'easeOutExpo');
		$('#sidebarblocker').stop().animate({opacity: '0.5'},300,'easeOutExpo');
	});
	
	$('#arrow').click(function(e) {
		$('#arrow').stop().animate({right: '300px'},300,'easeOutExpo');
		$('#sidebar').stop().animate({width: '300px'},300,'easeOutExpo');
		$('#sidebarblocker').stop().animate({opacity: '0.8'},300,'easeOutExpo');
		$('#sidebarcontainer').stop().delay(100).animate({right: '0px'},300,'easeOutExpo');
	});
	
	$('#sidebar').click(function(e) {
		$('#arrow').stop().animate({right: '300px'},300,'easeOutExpo');
		$('#sidebar').stop().animate({width: '300px'},300,'easeOutExpo');
		$('#sidebarblocker').stop().animate({opacity: '0.8'},300,'easeOutExpo');
		$('#sidebarcontainer').stop().delay(100).animate({right: '0px'},300,'easeOutExpo');
	});
	
	$('#arrow').mouseleave(function(e) {
		$('#arrow').stop().animate({right: '3px'},300,'easeOutExpo');
		$('#sidebar').stop().animate({width: '3px'},300,'easeOutExpo');
		$('#sidebarblocker').stop().animate({opacity: '0'},300,'easeOutExpo');
		$('#sidebarcontainer').stop().animate({right: '-300px'},300,'easeOutExpo');
	});
	
	$('#sidebar').mouseleave(function(e) {
		$('#arrow').stop().animate({right: '3px'},300,'easeOutExpo');
		$('#sidebar').stop().animate({width: '3px'},300,'easeOutExpo');
		$('#sidebarblocker').stop().animate({opacity: '0'},300,'easeOutExpo');
		$('#sidebarcontainer').stop().animate({right: '-300px'},300,'easeOutExpo');
	});
});

$(window).load(function(e) {
	//$('#srcimg').load(function() {
		iwidth = $("#srcimg").width();
		iheight = $("#srcimg").height();
		resize();
		$.unblockUI();
	//});
});

$(window).resize(function(e) {
    resize();
});

function resize() {
	swidth = $("#image").width()-100;
	sheight = $("#image").height()-100;
	
	widthRatio = iwidth/swidth;
	heightRatio = iheight/sheight;

	if(heightRatio > widthRatio) {
		$('#srcimg').height(sheight);
		$('#srcimg').width(iwidth*(sheight/iheight));
		log("1");
	} else {
		$('#srcimg').width(swidth);
		$('#srcimg').height(iheight*(swidth/iwidth));
		log(iheight);
	}
}

function log(message){
if(typeof console == "object"){
console.log(message);
}
}