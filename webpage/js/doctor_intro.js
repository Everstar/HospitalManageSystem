/**
 * Created by yclis on 7/21/2016.
 */

var uri = 'api/Patient/GetUserInfo';

var setData = function (name, sex, clinic, post, intro) {
    $("#employee_name").html(name);
    $("#employee_sex").html(sex);
    $("#employee_birth").html(clinic);
    $("#employee_cred_ID").html(post);
    $("#employee_department").html(intro);
}

$(document).ready(function () {
    // $("#avatar").attr("src", "../../image/avatars/1452667.jpg");

    var id = GetQueryString("id");
    console.log("id:" + id);
    if (id === null) {
        $("#employee_id").html("无此人");
        setData("null", "null", "null", "null", "null");
        return;
    }
    $("#employee_id").html(id);

//        var postData = {account: id};

    $.ajax({
        type: "get",
        url: "../../../api/Patient/GetSingleEmployee/" + id,
        contentType: "application/json",
//            data: JSON.stringify(),
        success: function (data, status) {
            var info = data;
            $("#avatar").attr("src", info.pic_url);
            $("#employee_name").val(info.name);
            $("#employee_sex").val(info.sex==="M"?"男":"女");
            $("#employee_clinic").val(info.clinic);
            $("#employee_post").val(info.post);
            $("#employee_intro").val(info.profile);
//                setData(info.name, info.sex, info.clinic, info.post, info.intro);
            for (var i in info.comment){
                var comment=info.comment[i];
                var row="<div class=\"itemdiv dialogdiv\">";
                row+="<div class=\"user\"><img alt=\"user image\" src=\"" +comment.pic_url+"\"/></div>";
                row+="<div class=\"body\">";
                row+="<div class=\"name\"><a href=\"#\">" + comment.name+"</a></div>";
                row+="<div class=\"time\">"+comment.time+"</div>";
                row+="<div class=\"text\">"+comment.content+"</div>";
                row+="<div class=\"tools\"><a href=\"#\" class=\"btn btn-minier btn-info\"><i class=\"icon-only icon-share-alt\"></i></a></div>";
                row+="</div></div>";
            }
        }
    });

});

