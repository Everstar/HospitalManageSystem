$(document).ready(function() {
    $.ajax({
        type: "get",
        url: '../../../api/ManagementStaff/GetAllEmployee',
        contentType: "application/json",
        success: function(data, status) {
            var jsonarray = $.parseJSON(data);


            $.each(jsonarray, function(i, n) {


                $.ajax({
                    type: "get",
                    url: "../../../api/Patient/GetEmployeeDutyTime/" + n.employee_id,
                    contentType: "application/json",
                    // data: postData,
                    // data: JSON.stringify(postData),
                    success: function(data, status) {
                    	console.log("saaa");
                    	console.log(data);
                    	data = $.parseJSON(data);
                        console.log(n.name);

                        var _monday_1 = data.Monday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _monday_2 = data.Monday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _monday_3 = data.Monday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';

                        var _tuesday_1 = data.Tuesday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _tuesday_2 = data.Tuesday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _tuesday_3 = data.Tuesday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';

                        var _wednesday_1 = data.Wednesday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _wednesday_2 = data.Wednesday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _wednesday_3 = data.Wednesday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';

                        var _thursday_1 = data.Thursday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _thursday_2 = data.Thursday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _thursday_3 = data.Thursday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';

                        var _friday_1 = data.Friday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _friday_2 = data.Friday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _friday_3 = data.Friday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';

                        var _saturday_1= data.Saturday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _saturday_2 = data.Saturday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _saturday_3 = data.Saturday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';

                        var _sunday_1 = data.Sunday.slice(0, 1) == "1" ? 'checked=\"checked\"' : '';
                        var _sunday_2 = data.Sunday.slice(1, 2) == "1" ? 'checked=\"checked\"' : '';
                        var _sunday_3 = data.Sunday.slice(2, 3) == "1" ? 'checked=\"checked\"' : '';


                        // console.log("1");
                        $("#duty_table").append(
                            "<tr>" +
                            "<td class='id'>" + n.employee_id + "</td>" +
                            "<td class='name'>" + n.name + "</td>" +
                            '<td class="room_num">' + data.room_num +
                            // '<select name="" id="" ><option value="">选择</option></select>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1" ' + _monday_1 + '/><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2" ' + _monday_2 + '/><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3" ' + _monday_3  + '/><label for="c1">晚</label>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1" ' + _tuesday_1 + '/><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2" ' + _tuesday_2 + ' /><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3" ' + _tuesday_3 +'/><label for="c1">晚</label>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1"' + _wednesday_1 + ' /><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2" ' + _wednesday_2 + '/><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3" ' + _wednesday_3 + '/><label for="c1">晚</label>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1" ' + _thursday_1 + '/><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2"  ' + _thursday_2 + '/><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3" ' + _thursday_3 + '/><label for="c1">晚</label>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1" ' + _friday_3 + '/><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2" ' + _friday_2 + ' /><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3"  ' + _friday_3 + '/><label for="c1">晚</label>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1" ' + _saturday_1 +' /><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2" ' +  _saturday_2+ '/><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3"' +  _saturday_3 + ' /><label for="c1">晚</label>' +
                            '</td>' +
                            '<td><input type="checkbox" name="" value="1" id="c1" ' + _sunday_1 + '/><label for="c1">早</label>' +
                            '<input type="checkbox" name="" value="1" id="c2" ' + _sunday_2 +'/><label for="c1">中</label>' +
                            '<input type="checkbox" name="" value="1" id="c3" '+ _sunday_3 +'/><label for="c1">晚</label>' +
                            '</td>' + 
                            '<button class=\"submit\">修改</button>'
                        );
                    }
                });

                // console.log($("#clinic_name option:selected").html());

            });
        }
    });




    // var uri = '../../../api/ManagementStaff/SetDutyTime';

    // 	data={
    // 		employee_id:
    // 		room_num:
    // 		Monday:
    // 		Tuesday:
    // 		Wednesday:
    // 		Thursday:
    // 		Friday:
    // 		Saturday:
    // 		Sunday:

    // 	}
    // 	$.ajax({
    //            type: "get",
    //            url: uri,
    //            contentType: "application/json",
    //            success: function(data, status) { 
    // 				var jsonarray= $.parseJSON(data);

    //            	console.log(data);

    // 			$.each(jsonarray,function (i, n){
    // 				$(".complaint_table").append(
    // 		    		"<tr>"+
    // 			    	"<td class='id'>"+n.department+"</td>"+
    // 				    "<td class='name'>"+n.clinic+"</td>"+
    // 				    "<td class='department'>"+n.employee_id+"</td>"+
    // 				    "<td class='clinic'>"+n.name+"</td>"+
    // 				    "<td class='doctor'>"+n.complaint_rate+"</td>"+
    // 				    "<td><a class='btn btn-primary' href='complaint_detail.html'>查看</a></td>"+
    // 				        "</tr>"
    // 			    	);

    // 			});
    // 		}
    //        });

});
