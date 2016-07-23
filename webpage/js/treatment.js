/**
 * Created by yclis on 7/20/2016.
 */

var medicineInfo;

$("document").ready(function () {
    $("#add-medicine").click(function () {
        console.log($("#medicine-name-list").val() + "  " + $("#medicine-number").val());
        if ($("#medicine-name-list").val() === null) {
            alert("请选择药品名称");
            return;
        }
        if ($("#medicine-number").val().length === 0) {
            // console.log($("#medicine-number").val());
            alert("药品数量不能为空");
        } else {
            var i = 0;
            var medicineName = $("#medicine-name-list").val();
            var unit;
            for (i = 0; i < medicineInfo.length; i++) {
                if (medicineInfo[i].medicine_id === medicineName) {
                    unit = medicineInfo[i].unit;
                    break;
                }
            }
            var newInsertion = "<tr><td>" + medicineInfo[i].name + "</td>" +
                "<td>" + $("#medicine-number").val() + "</td>" +
                "<td>" + unit + "</td>"
                + "</tr>";
            for (i in $("#medicine-name-list").children){
                if ($("#medicine-name-list")[i].val()===medicineName){
                    $("#medicine-name-list")[i].remove();
                    break;
                }
            }

            $("#presciption-table").append(newInsertion);
        }
    });

    $("#submitPrescription").click(function () {
        // var prescriptionArray = [1234];
        var prescriptionArray = [$("#treatment-id-in-prescription").val()];
        var tbody = document.getElementById("presciption-table");
        var trs = tbody.getElementsByTagName("tr"); //获取tbody元素下的所有tr元素
        for (var i = 0; i < trs.length; i++) {     //循环 tr元素
            // trs[i].style.fontSize = "30px";// 改变符合条件的tr元素的背景色
            var tds = trs[i].getElementsByTagName("td");
            var object = {};
            for (var j = 0; j < medicineInfo.length; j++) {
                if (medicineInfo[j].name === tds[0].innerText) {
                    object.id = medicineInfo[j].medicine_id;
                    break;
                }
            }
            object.number = tds[1].innerText;
            prescriptionArray.push(object);
        }

        $.ajax({
            type: "post",
            url: "../../../api/Doctor/WritePrescription",
            contentType: "application/json",
            data: JSON.stringify(prescriptionArray),
            success: function (data, status) {
                alert("提交成功");
                window.location.reload();
            }
        });
    });

    $("#submit-exam-list").click(function () {
        var treatment_id = $("#treatment-id-in-examinaton").val();
        var exam_type = $("#examine-list").val();
        if(exam_type == null) {
            alert("请选择检查类型！");
            return;
        }

        $.ajax({
            type : "get",
            url: "../../../api/Doctor/WriteExamination/" + treatment_id + "/" + exam_type,
            success: function(data, status) {
                alert("提交成功");
                window.location.reload();
            },
            error : function(XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest + textStatus + errorThrown);
            }
        });
    });
    
    $("#submit-surgey").click(function () {
        var treatment_id = $("#treatment-id-in-surgey").val();
        var surgey_name= $("#surgey-name").val();
        // $.getJSON("../../../api/Doctor/WriteSurgey/" + treatment_id + "/" + surgey_name)
        //     .done(function (data) {
        //         alert("提交成功");
        //         window.location.reload();
        //     })
        //     .fail(function(){
        //         alert("提交失败");
        //     })

        $.ajax({
            type : "get",
            url: "../../../api/Doctor/WriteSurgey/" + treatment_id + "/" + surgey_name,
            success: function(data, status) {
                alert("提交成功");
                window.location.reload();
            },
            error : function(XMLHttpRequest, textStatus, errorThrown) {
                alert("提交失败")
                console.log(XMLHttpRequest + textStatus + errorThrown);
            }
        });
    });

    $("#submit-hospitalization").click(function () {
        var treatment_id=$("#treatment-id-in-hospitalization").val();
        // $.getJSON("../../../api/Doctor/WriteHospitalization/" +treatment_id)
        //     .done(function (data) {
        //         alert("提交成功");
        //         window.location.reload();
        //     })
        //     .fail(function () {
        //         alert("提交失败");
        //     });

        $.ajax({
            type : "get",
            url: "../../../api/Doctor/WriteHospitalization/" +treatment_id,
            success: function(data, status) {
                alert("提交成功");
                window.location.reload();
            },
            error : function(XMLHttpRequest, textStatus, errorThrown) {
                alert("提交失败")
                console.log(XMLHttpRequest + textStatus + errorThrown);
            }
        });
    });



    var button_operation =
        "<button class=\"blue btn-prescription\">开处方</button> <button class=\"blue btn-examination\">开检查</button> <button class=\"blue btn-operation\">安排手术</button> <button class=\"blue btn-hospitalization\">安排住院</button>";
    var addPrescriptionClickListener = function () {
        $(".btn-prescription").click(function () {
            var treatment_id = $(this).parent().parent().parent().children().eq(0).text();
            $("#treatment-id-in-prescription").val(treatment_id);
            $.getJSON("../../../api/Doctor/GetMedicineList")
                .done(function (data) {
                    medicineInfo = data;
                    console.log(medicineInfo);
                    for (var i = 0; i < data.length; i++) {
                        var row = "<option value=\"" + data[i].medicine_id + "\">" + data[i].name + "</option>";
                        $("#medicine-name-list").append(row);
                    }
                });
            $("#list-of-registraiton").hide();
            $("#write-prescription").show();
        })
    };
    var addExaminationClickListener = function () {
        $(".btn-examination").click(function () {
            var treatment_id = $(this).parent().parent().parent().children().eq(0).text();
            $("#treatment-id-in-examinaton").val(treatment_id);
            $("#list-of-registraiton").hide();
            $("#write-examination").show();
        })
    };
    var addOperationClickListener = function () {
        $(".btn-operation").click(function () {
            var treatment_id = $(this).parent().parent().parent().children().eq(0).text();
            $("#treatment-id-in-surgey").val(treatment_id);
            // console.log(treatment_id);
            $("#list-of-registraiton").hide();
            $("#write-surgery").show();
        });
    };
    var addHospitalizationClickListener = function () {
        $(".btn-hospitalization").click(function () {
            var treatment_id = $(this).parent().parent().parent().children().eq(0).text();
            $("#treatment-id-in-hospitalization").val(treatment_id);
            $("#list-of-registraiton").hide();
            $("#write-hospitalization").show();
        })
    };
    var addClickListener = function () {
        // 按下接诊
        console.log("push down");
        $(".button_takes").click(function () {
            var treatment_id = $(this).parent().parent().parent().children().eq(0).text();
            var label = $(this).parent().parent().parent().find(".label_takes").find("span");
            label.removeClass("label-warning");
            label.addClass("label-success");
            label.text("已接诊");

            var buttonGroup = $(this).parent();
            $(this).remove();
            buttonGroup.append(button_operation);
            addPrescriptionClickListener();
            addExaminationClickListener();
            addOperationClickListener();
            addHospitalizationClickListener();
            $.ajax({
                type : "get",
                url: "../../../api/Doctor/TakesRegistration/" + treatment_id,
                success: function(data, status) {
                    console.log(data);

                },
                error : function(XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                }
            });
            // $.getJSON("../../../api/Doctor/TakesRegistration/" + treatment_id)
            //     .done(function (data) {
            //         var label = $(this).parent().parent().parent().find(".label_takes").find("span");
            //         label.removeClass("label-warning");
            //         label.addClass("label-success");
            //         label.text("已接诊");

            //         var buttonGroup = $(this).parent();
            //         $(this).remove();
            //         buttonGroup.append(button_operation);
            //         addPrescriptionClickListener();
            //         addExaminationClickListener();
            //         addOperationClickListener();
            //         addHospitalizationClickListener();
            //     })
            //     .fail(function (data) {
            //         // alert(data);
            //         console.log(data);
            //     });
        });

    };
    addClickListener();
    addPrescriptionClickListener();
    addExaminationClickListener();
    addOperationClickListener();
    addHospitalizationClickListener();


    $.getJSON("../../../api/Doctor/GetAllTreatment")
        .done(function (data) {
            console.log(data);
            if (data[0] === "1") {
                for (var i = 1; i < data.length; i++) {
                    var row = "<tr>";
                    row += "<td>" + data[i].treatment_id + "</td>";
                    row += "<td>" + data[i].employee_name + "</td>";
                    row += "<td>" + data[i].clinic + "</td>";
                    row += "<td>" + data[i].patient_name + "</td>";
                    row += "<td>" + data[i].start_time.slice(0, 9) + data[i].start_time.slice(11, 18) + "</td>";
                    if (data[i].take === "1") {
                        row += "<td class=\"label_takes\"><span class=\"label label-sm label-success\">" + "已接诊" + "</span></td>" +
                            "<td><div class=\"visible-md visible-lg hidden-sm hidden-xs action-buttons\">" + button_operation + "</div></td>";
                    } else {
                        row += "<td class=\"label_takes\"><span class=\"label label-sm label-warning\">" + "未接诊" + "</span></td>" +
                            "<td><div class=\"visible-md visible-lg hidden-sm hidden-xs action-buttons\"><button class=\"btn-success button_takes\">接诊</button></div></td>";
                    }
                    row += "</tr>";
                    $("#treatmentRecord").append(row);

                }
                addClickListener();
                addPrescriptionClickListener();
                addExaminationClickListener();
                addOperationClickListener();
                addHospitalizationClickListener();
            } else {
                alert("当前无记录");
            }
        })
        .fail(function () {
            alert("载入记录失败");
        });
})