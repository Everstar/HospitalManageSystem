$(document).ready(function(){
	//alert("hello world");
	$(".siderbar>ul>li").mouseover(function(){
		$(this).find("a").addClass("a_mouseover").removeClass("a_mouseout");
		$(this).find("a").css("font-size","20px");
	}).mouseout(function(){
		$(this).find("a").addClass("a_mouseout").removeClass('a_mouseover');
		$(this).find("a").css("font-size","18px");

	});
});