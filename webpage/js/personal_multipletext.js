$(document).ready(function(){

	

	var uri = '../../../api/ManagementStaff/GetAllEmployee';

	$.ajax({
            type: "get",
            url: uri,
            contentType: "application/json",
            success: function(data, status) {
				var jsonarray= $.parseJSON(data);


				$.each(jsonarray,function (i, n)
						{
							// console.log($("#clinic_name option:selected").html());
							console.log(n.name);
					    		// console.log("1");
					    		$("#imployee_table>tbody").append(
						    		"<tr>"+
							    	"<td class='id'>"+n.employee_id+"</td>"+
								    "<td class='name'>"+n.name+"</td>"+
								    "<td class='department'>"+n.department+"</td>"+
								    "<td class='clinic'>"+n.clinic+"</td>"+
								    "<td class='doctor'>"+n.post+"</td>"+
								    "<td class='salary'>"+n.salary+"</td>"+
								    "<td class='change'><a class='btn btn-primary personal_change'>修改</a>"+
							  				   "<a class='btn btn-primary personal_delete'>删除</a></td>"+
		     				        "</tr>"
		     				    );
						});
				// console.log(data);
            	$("#department_name").change(function(){
				var dpt_name =$(this).find(":selected").html();
				if(dpt_name =="行政部"){
					$("#clinic_name").children().remove();//.append("<option>行政部</option>");
					$("#clinic_name").append("<option>科室</option>").append("<option>行政科</option>").append("<option>财务科</option>");
					$("#clinic_name").change(function(){
						$.each(jsonarray,function (i, n)
						{
							console.log($("#clinic_name option:selected").html());
					    	if(n.clinic==$("#clinic_name option:selected").html()){
					    		// console.log("1");
					    		$("#imployee_table>tbody>tr:gt(0)").remove();
					    		$("#imployee_table>tbody").append(
						    		"<tr>"+
							    	"<td class='id'>"+n.employee_id+"</td>"+
								    "<td class='name'>"+n.name+"</td>"+
								    "<td class='department'>"+n.department+"</td>"+
								    "<td class='clinic'>"+n.clinic+"</td>"+
								    "<td class='doctor'>"+n.post+"</td>"+
								    "<td class='salary'>"+n.salary+"</td>"+
								    "<td class='change'><a class='btn btn-primary personal_change'>修改</a>"+
							  				   "<a class='btn btn-primary personal_delete'>删除</a></td>"+
		     				        "</tr>"
		     				    );
							}    
						});
					});
					
				}else if(dpt_name=="后勤部"){
					$("#clinic_name").children().remove();
					$("#clinic_name").append("<option>科室</option>");
					$("#clinic_name").append("<option>后勤部</option>");
					
					$("#clinic_name").change(function(){
						$.each(jsonarray,function (i, n)
						{
							console.log($("#clinic_name option:selected").html());
					    	if(n.department=="后勤部"){
					    		// console.log("1");
					    		$("#imployee_table>tbody>tr:gt(0)").remove();
					    		$("#imployee_table>tbody").append(
						    		"<tr>"+
							    	"<td class='id'>"+n.employee_id+"</td>"+
								    "<td class='name'>"+n.name+"</td>"+
								    "<td class='department'>"+n.department+"</td>"+
								    "<td class='clinic'>"+n.clinic+"</td>"+
								    "<td class='doctor'>"+n.post+"</td>"+
								    "<td class='salary'>"+n.salary+"</td>"+
								    "<td class='change'><a class='btn btn-primary personal_change'>修改</a>"+
							  				   "<a class='btn btn-primary personal_delete'>删除</a></td>"+
		     				        "</tr>"
		     				    );
							}    
						});
					});
				}else if(dpt_name=="门诊部"){
					$("#clinic_name").children().remove();
					$("#clinic_name").append("<option>科室</option>");
					$("#clinic_name").append("<option>药剂科</option>");
					$("#clinic_name").append("<option>检测科</option>");
					$("#clinic_name").append("<option>内科</option>");
					$("#clinic_name").append("<option>儿科</option>");
					$("#clinic_name").change(function(){
						$.each(jsonarray,function (i, n)
						{
							var clinic = $("#clinic_name option:selected").html();
							// console.log(clinic);
					    	if(n.clinic==clinic){
					    		console.log("1");
					    		$("#imployee_table>tbody>tr:gt(0)").remove();
					    		$("#imployee_table>tbody").append(
						    		"<tr>"+
							    	"<td class='id'>"+n.employee_id+"</td>"+
								    "<td class='name'>"+n.name+"</td>"+
								    "<td class='department'>"+n.department+"</td>"+
								    "<td class='clinic'>"+n.clinic+"</td>"+
								    "<td class='doctor'>"+n.post+"</td>"+
								    "<td class='salary'>"+n.salary+"</td>"+
								    "<td class='change'><a class='btn btn-primary personal_change'>修改</a>"+
							  				   "<a class='btn btn-primary personal_delete'>删除</a></td>"+
		     				        "</tr>"
		     				    );
							}    
						});
					});
				}
			});

            	// console.log(data);
				
            	


                
            }
        });


	// $(".personal_change").click(function() {
	// 	/* Act on the event */
	// 	if($(this).text()=="修改"){
	// 		// var id=$(this).parent().parent().find(".id").text();
	// 		// var name=$(this).parent().parent().find(".name").text();
	// 		var department=$(this).parent().parent().find(".department").text();
	// 		var clinic=$(this).parent().parent().find(".clinic").text();
	// 		var doctor=$(this).parent().parent().find(".doctor").text();
	// 		var salary=$(this).parent().parent().find(".salary").text();

	// 		// $(this).parent().parent().find(".id").text("").append("<input type='text' class='col-xs-8' value='"+id+"' />");
	// 		// $(this).parent().parent().find(".name").text("").append("<input type='text' class='col-xs-8'value='"+name+"' />");
	// 		$(this).parent().parent().find(".department").text("").append("<input type='text' class='col-xs-8'value='"+department+"' />");
	// 		$(this).parent().parent().find(".clinic").text("").append("<input type='text' class='col-xs-8'value='"+clinic+"' />");
	// 		$(this).parent().parent().find(".doctor").text("").append("<input type='text' class='col-xs-8'value='"+doctor+"' />");
	// 		$(this).parent().parent().find(".salary").text("").append("<input type='text' class='col-xs-8'value='"+salary+"' />");

	// 	}else{

	// 		// var id=$(this).parent().parent().find(".id>input").val();
	// 		// var name=$(this).parent().parent().find(".name>input").val();
	// 		var department=$(this).parent().parent().find(".department>input").val();
	// 		var clinic=$(this).parent().parent().find(".clinic>input").val();
	// 		var doctor=$(this).parent().parent().find(".doctor>input").val();
	// 		var salary=$(this).parent().parent().find(".salary>input").val();

	// 		$(this).text("修改");

	// 		// $(this).parent().parent().find(".id").text(id).children().remove();
	// 		// $(this).parent().parent().find(".name").text(name).children().remove();
	// 		$(this).parent().parent().find(".department").text(department).children().remove();
	// 		$(this).parent().parent().find(".clinic").text(clinic).children().remove();
	// 		$(this).parent().parent().find(".doctor").text(doctor).children().remove();
	// 		$(this).parent().parent().find(".salary").text(salary).children().remove();

	// 	}
	//  });
	//  
	$("#imployee_table").click(function(e) {
		console.log("clicked!");
		console.log($(e.target).text());
       	if($(e.target).text()==="修改"){
       		console.log("修改 clicked");
       		$(e.target).text("提交");
			// var id=$(this).parent().parent().find(".id").text();
			// var name=$(this).parent().parent().find(".name").text();
			var _id = $(e.target).parent().parent().find(".id").text();
			var _department=$(e.target).parent().parent().find(".department").text();
			var _clinic=$(e.target).parent().parent().find(".clinic").text();
			var _doctor=$(e.target).parent().parent().find(".doctor").text();
			var _salary=$(e.target).parent().parent().find(".salary").text();

			// $(this).parent().parent().find(".id").text("").append("<input type='text' class='col-xs-8' value='"+id+"' />");
			// $(this).parent().parent().find(".name").text("").append("<input type='text' class='col-xs-8'value='"+name+"' />");
			$(e.target).parent().parent().find(".department").text("").append("<input type='text' class='col-xs-8'value='"+_department+"' />");
			$(e.target).parent().parent().find(".clinic").text("").append("<input type='text' class='col-xs-8'value='"+_clinic+"' />");
			$(e.target).parent().parent().find(".doctor").text("").append("<input type='text' class='col-xs-8'value='"+_doctor+"' />");
			$(e.target).parent().parent().find(".salary").text("").append("<input type='text' class='col-xs-8'value='"+_salary+"' />");

			var postData = {id : _id, post : _doctor, clinic : _clinic, salary : _salary, dept_name : _salary};

			// $.ajax({
	  //           type: "post",
   //  	        url: "../../../api/ManagementStaff/SetEmployee",
   //      	    contentType: "application/json",
   //              data: JSON.stringify(postData),
   //          	success: function(data, status) {
			// 		// var jsonarray= $.parseJSON(data);
			// 		alert(data);
			// 	}
			
			// });

       }else if($(e.target).text()==="提交"){

	       	console.log("提交 clicked");
	       	var uri = '../../../api/ManagementStaff/SetEmployee';
			var _id = $(e.target).parent().parent().find(".id").text();
			var _department=$(e.target).parent().parent().find(".department").find("input").val();
			var _clinic=$(e.target).parent().parent().find(".clinic").find("input").val();
			var _doctor=$(e.target).parent().parent().find(".doctor").find("input").val();
			var _salary=$(e.target).parent().parent().find(".salary").find("input").val();

			var postData = {id : _id, post : _doctor, clinic_name : _clinic, salary : _salary, dept_name : _department};
			$.ajax({
	            type: "post",
    	        url: uri,
        	    contentType: "application/json",
                data: JSON.stringify(postData),
            	success: function(data, status) {
					// var jsonarray= $.parseJSON(data);
					console.log("成功");
				}
			
			});


			// var id=$(this).parent().parent().find(".id>input").val();
			// var name=$(this).parent().parent().find(".name>input").val();
			var department=$(e.target).parent().parent().find(".department>input").val();
			var clinic=$(e.target).parent().parent().find(".clinic>input").val();
			var doctor=$(e.target).parent().parent().find(".doctor>input").val();
			var salary=$(e.target).parent().parent().find(".salary>input").val();

			$(e.target).text("修改");

			// $(this).parent().parent().find(".id").text(id).children().remove();
			// $(this).parent().parent().find(".name").text(name).children().remove();
			$(e.target).parent().parent().find(".department").text(department).children().remove();
			$(e.target).parent().parent().find(".clinic").text(clinic).children().remove();
			$(e.target).parent().parent().find(".doctor").text(doctor).children().remove();
			$(e.target).parent().parent().find(".salary").text(salary).children().remove();

		

       }else if($(e.target).text()==="删除"){
       		$(e.target).parent().parent().remove();
       		var uri = '../../../api/ManagementStaff/DeleteEmployee/' + $(e.target).parent().parent().find(".id").html();
	       	
			$.ajax({
	            type: "get",
    	        url: uri,
        	    contentType: "application/json",
            	success: function(data, status) {
					// var jsonarray= $.parseJSON(data);
					console.log(data);
				}
			
			});

       };
   	});



});

// var personal_change = function() {
// 		/* Act on the event */
// 		if($(this).text()=="修改"){
// 			$(this).text("提交");
// 			// var id=$(this).parent().parent().find(".id").text();
// 			// var name=$(this).parent().parent().find(".name").text();
// 			var department=$(this).parent().parent().find(".department").text();
// 			var clinic=$(this).parent().parent().find(".clinic").text();
// 			var doctor=$(this).parent().parent().find(".doctor").text();
// 			var salary=$(this).parent().parent().find(".salary").text();

// 			// $(this).parent().parent().find(".id").text("").append("<input type='text' class='col-xs-8' value='"+id+"' />");
// 			// $(this).parent().parent().find(".name").text("").append("<input type='text' class='col-xs-8'value='"+name+"' />");
// 			$(this).parent().parent().find(".department").text("").append("<input type='text' class='col-xs-8'value='"+department+"' />");
// 			$(this).parent().parent().find(".clinic").text("").append("<input type='text' class='col-xs-8'value='"+clinic+"' />");
// 			$(this).parent().parent().find(".doctor").text("").append("<input type='text' class='col-xs-8'value='"+doctor+"' />");
// 			$(this).parent().parent().find(".salary").text("").append("<input type='text' class='col-xs-8'value='"+salary+"' />");

// 		}else{

// 			// var id=$(this).parent().parent().find(".id>input").val();
// 			// var name=$(this).parent().parent().find(".name>input").val();
// 			var department=$(this).parent().parent().find(".department>input").val();
// 			var clinic=$(this).parent().parent().find(".clinic>input").val();
// 			var doctor=$(this).parent().parent().find(".doctor>input").val();
// 			var salary=$(this).parent().parent().find(".salary>input").val();

// 			$(this).text("修改");

// 			// $(this).parent().parent().find(".id").text(id).children().remove();
// 			// $(this).parent().parent().find(".name").text(name).children().remove();
// 			$(this).parent().parent().find(".department").text(department).children().remove();
// 			$(this).parent().parent().find(".clinic").text(clinic).children().remove();
// 			$(this).parent().parent().find(".doctor").text(doctor).children().remove();
// 			$(this).parent().parent().find(".salary").text(salary).children().remove();

// 		}
// 	 }
