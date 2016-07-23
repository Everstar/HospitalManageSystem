$(document).ready(function(){
	//alert("hello world");
	// console.log("写权限测试");
	for(year=2016;year>1960;year--){
		value=2017-year;

		$(".complaint_rate_year").append("<option value='"+value+"'>"+year+"</option>");
	}
	for(month=1;month<13;month++){

		$(".complaint_rate_month").append("<option value='"+month+"'>"+month+"</option>");
	}


	for(percent_begin=0;percent_begin<101;percent_begin++){

		$(".complaint_rate_begin").append("<option value='"+percent_begin+"'>"+percent_begin+"%</option>");
	}

	$(".complaint_rate_begin").change(
		function(){
			start=$(this).val();
			$(".complaint_rate_end").children().remove();
			for(percent_end=start;percent_end<101;percent_end++){
				$(".complaint_rate_end").append("<option value='"+percent_end+"'>"+percent_end+"%</option>");
			}
		}
	); 

	
	


	$(".siderbar>ul>li").mouseover(function(){
		$(this).find("a").addClass("a_mouseover").removeClass("a_mouseout");
		$(this).find("a").css("font-size","20px");
	}).mouseout(function(){
		$(this).find("a").addClass("a_mouseout").removeClass('a_mouseover');
		$(this).find("a").css("font-size","18px");
	});

	

 
});

