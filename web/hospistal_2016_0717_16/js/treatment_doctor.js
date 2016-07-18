// js for manage treatment record in doctor role

$(function() {
	var button_operation = "<td><div class=\"visible-md visible-lg hidden-sm hidden-xs action-buttons\"><button class=\"blue\">开处方</button> <button class=\"blue\">开检查</button> <button class=\"blue\">安排手术</button> <button class=\"blue\">安排住院</button></div></td>";

	var addClickListern = function() {
			// 按下接诊
			$(".button_takes").click(function(){
				var label = $(this).parent().parent().parent().find(".label_takes").find("span");
				label.removeClass("label-warning");
				label.addClass("label-success");
				label.text("已接诊");

				var buttonGroup = $(this).parent();
				$(this).remove();
				buttonGroup.append(button_operation);
			});

	}

	addClickListern();

	// 随机生成新纪录
	$(".table-header").on("click", function() {
		var TDnumber = "<td>2016071622210" + (Math.random() * 10000012).toPrecision(7) + "</td>";
		var TDname = "<td>" + "某某" + "</td>";
		var TDclinic = "<td>" + "皮肤科" + "</td>";
		var TDdoctor = "<td>" + "许迪文" + "</td>";
		var TDsubmitTime = "<td>" + "2016/07/17 16:02" + "</td>";
		var TDstatus = "";
		var TDoperation = "";

		if(Math.random() > 0.5) {
			// 未接诊
			TDstatus = "<td class=\"label_takes\"><span class=\"label label-sm label-warning\">" + "未接诊" + "</span></td>";
			TDoperation = "<td><div class=\"visible-md visible-lg hidden-sm hidden-xs action-buttons\"><button class=\"btn-success button_takes\">接诊</button></div></td>";	
		} else {
			// 已接诊
			TDstatus = "<td class=\"label_takes\"><span class=\"label label-sm label-success\">" + "已接诊" + "</span></td>";
			TDoperation = button_operation;
		}

		var TDinsert = "<tr>" + TDnumber + TDname + TDclinic + TDdoctor + TDsubmitTime + TDstatus + TDoperation + "</tr>";
		$("#treatmentRecord").append(TDinsert);
		addClickListern();
	});

});