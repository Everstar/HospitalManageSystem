/**
 * Created by yclis on 7/20/2016.
 */

// var selectedDoctor;
var allDoctorInfo;
var doctorId;

$(document).ready(function () {

    var d = new Date();
    var startdate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
    var enddate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + (d.getDate() + 7);
    $('#datetimepicker').datetimepicker('setStartDate', startdate);
    $('#datetimepicker').datetimepicker('setEndDate', enddate);

    // 获取所有科室
    $.getJSON("../../../api/Patient/GetAllClinic")
        .done(function (data) {
            for (var str in data) {
                var row = "<option value=\"" + data[str] + "\">" + data[str] + "</option>";
                $("#clinic-name-list").append(row);
            }
        })
        .fail(function (data) {
            alert(data);
        });

    // 按钮跳转
    $("#btn-jump-to-choose-doctor").click(function () {
        var clinicName = $("#clinic-name-list").val();
        // console.log(clinicName);
        if (clinicName === null) {
            alert("请选择科室");
            return;
        }
        $.getJSON("../../../api/Patient/GetEmployee/" + clinicName)
            .done(function (data) {
                allDoctorInfo = data;
                for (var i in data) {
                    var singleDoctorInfo = data[i];
                    var row = "<tr>";
                    row += "<td><div class=\"col-sm-6\"><img src=\"" + singleDoctorInfo.pic_url + "\" class=\"img-circle\" alt=\"医生 头像\" width=\"80\"></div>" +
                        "<div><p><a href=\"./doctor_intro.html?id=" + singleDoctorInfo.employee_id + "\" target=\"_blank\">" + singleDoctorInfo.name + "</a></p><p>" + singleDoctorInfo.post + "</p></div></td>";
                    row += "<td><p>" + singleDoctorInfo.skill + "</p></td>";
                    row += "<td><p>平均星级：" + (singleDoctorInfo.rank === -1 ? "暂无评价" : singleDoctorInfo.rank) + "</p></td>";
                    row += "<td><div class=\"center\"><button class=\"btn btn-primary btn-jump-to-choose-time\">挂号</button></div></td>";
                    row += "</tr>";
                    $("#doctor-table").append(row);
                    // console.log(row);
                }

                $(".btn-jump-to-choose-time").click(function () {
                    var doctorName = $(this).parent().parent().parent().children().eq(0).children().eq(1).children().eq(0).children().eq(0).text();
                    console.log(doctorName);
                    // console.log("length: "+allDoctorInfo.length);
                    for (i in allDoctorInfo) {
                        var row = allDoctorInfo[i];
                        console.log(i + " " + row.name);
                        if (row.name === doctorName) {
                            doctorId = row.employee_id;
                        }
                    }
                    $("#content-choose-doctor").hide();
                    $("#content-choose-time").show();
                });
            });
        $("#content-choose-clinic").hide();
        $("#content-choose-doctor").show();
    });

    $(".btn-jump-to-choose-time").click(function () {
        var doctorName = $(this).parent().parent().parent().children().eq(0).children().eq(1).children().eq(0).children().eq(0).text();
        console.log("length: " + allDoctorInfo.length);
        for (i in allDoctorInfo) {
            var row = allDoctorInfo[i];
            console.log(i + " " + row.name);
            if (row.name === doctorName) {
                doctorId = row.employee_id;
            }
        }
        $("#content-choose-doctor").hide();
        $("#content-choose-time").show();
    });

    $("#submit-register").click(function () {
        var time = $("#datetimepicker").val() + ":00";
        console.log($("#datetimepicker").val() + ":00");
        console.log(doctorId);
        $.ajax({
            type: "post",
            url: "../../../api/Patient/Register",
            contentType: "application/json",
            data: JSON.stringify({
                "time": time,
                "doctorId": doctorId
            }),
            success: function (data, status) {
                alert("挂号成功");
            }
            // content
        });

        // $.getJSON("../../../api/Patient/Register")
        //     .done(function (data) {
        //
        //     });
    });


});