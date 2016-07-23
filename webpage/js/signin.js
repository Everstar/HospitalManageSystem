//javascript for sigin.html

$(function() {
    var signin = function(input_account, input_password) {
        var uri = '../../api/Account/SignIn';

        console.log(input_account + "  " + input_password );

        var postData = { account: input_account, passwd: input_password };

        $.ajax({
            type: "post",
            url: uri,
            contentType: "application/json",
            data: JSON.stringify(postData),
            success: function(data, status) {
                //正确则跳转
                var user_type = data.toString();
                if(user_type === "Patient") {
                    window.location.href="./patient/user_info.html";
                }else if(user_type === "Doctor"){
                    window.location.href="./doctor/user_info.html";
                }else if(user_type === "Nurse") {
                    window.location.href="./nurse/user_info.html";
                }else if(user_type === "Examiner") {
                    window.location.href="./examiner/user_info.html";
                }else if(user_type === "ManagementStaff") {
                    window.location.href="./managementstaff/user_info.html";
                }else if(user_type === "Pharmacist") {
                    window.location.href="./pharmacist/user_info.html";
                }else {
                    alert("Error");
                }
                
            }
        });
    }

    var signup = function(input_name, input_ID, input_birth, input_password) {
        var uri = '../../api/Account/SignUp';

        if(input_ID.length == 18)
            var input_sex = (input_ID.slice(-2, -1) % 2) === 1 ? "M" : "F";
        else {
            alert("请输入有效身份证号。");
            return;
        }

        console.log(input_name + "  "+ input_ID + "  " + input_sex + " " + input_birth +  "  " + input_password);

        var postData = { name: input_name, sex : input_sex, credit_num : input_ID, birth : input_birth.toString() , passwd: input_password };

        $.ajax({
            type: "post",
            url: uri,
            contentType: "application/json",
            data: JSON.stringify(postData),
            success: function(data, status) {
                var patient_id = data;
                alert("你的病人账号为 :" + patient_id + "\n请重新登录。");
                window.location.reload();
            }
        });
    }

    $("#button_signin").click(function() {
        var account = $("#name").val();
        var password = $("#pass").val();
        signin(account, password);
    });

    $("#button_signup").click(function() {
        var name = $("#regname").val();
        var cred_ID = $("#regID").val();
        var birth = $("#regbirth").val();
        var password = $("#regpass").val();
        signup(name, cred_ID, birth, password);
    });


    $(".input input").focus(function() {

        $(this).parent(".input").each(function() {
            if ($(this).index() === 4)
                return;
            $("label", this).css({
                "line-height": "18px",
                "font-size": "18px",
                "font-weight": "100",
                "top": "0px"
            })
            $(".spin", this).css({
                "width": "100%"
            })
        });
    }).blur(function() {
        if ($(this).index() === 1)
            return;
        $(".spin").css({
            "width": "0px"
        })
        if ($(this).val() == "") {
            $(this).parent(".input").each(function() {
                $("label", this).css({
                    "line-height": "60px",
                    "font-size": "24px",
                    "font-weight": "300",
                    "top": "10px"
                })
            });

        }
    });

    $(".button").click(function(e) {
        var pX = e.pageX,
            pY = e.pageY,
            oX = parseInt($(this).offset().left),
            oY = parseInt($(this).offset().top);

        $(this).append('<span class="click-efect x-' + oX + ' y-' + oY + '" style="margin-left:' + (pX - oX) + 'px;margin-top:' + (pY - oY) + 'px;"></span>')
        $('.x-' + oX + '.y-' + oY + '').animate({
            "width": "500px",
            "height": "500px",
            "top": "-250px",
            "left": "-250px",

        }, 600);
        $("button", this).addClass('active');
    })

    $(".alt-2").click(function() {
        if (!$(this).hasClass('material-button')) {
            $(".shape").css({
                "width": "100%",
                "height": "100%",
                "transform": "rotate(0deg)"
            })

            setTimeout(function() {
                $(".overbox").css({
                    "overflow": "initial"
                })
            }, 600)

            $(this).animate({
                "width": "120px",
                "height": "120px"
            }, 500, function() {
                $(".box").removeClass("back");

                $(this).removeClass('active')
            });

            $(".overbox .title").fadeOut(300);
            $(".overbox .input").fadeOut(300);
            $(".overbox .button").fadeOut(300);

            $(".alt-2").addClass('material-buton');
        }

    })

    $(".material-button").click(function() {

        if ($(this).hasClass('material-button')) {
            setTimeout(function() {
                $(".overbox").css({
                    "overflow": "hidden"
                })
                $(".box").addClass("back");
            }, 200)
            $(this).addClass('active').animate({
                "width": "700px",
                "height": "800px",
            });

            setTimeout(function() {
                $(".shape").css({
                    "width": "50%",
                    "height": "50%",
                    "transform": "rotate(45deg)"
                })

                $(".overbox .title").fadeIn(300);
                $(".overbox .input").fadeIn(300);
                $(".overbox .button").fadeIn(300);
            }, 700)

            $(this).removeClass('material-button');

        }

        if ($(".alt-2").hasClass('material-buton')) {
            $(".alt-2").removeClass('material-buton');
            $(".alt-2").addClass('material-button');
        }

    });

});
