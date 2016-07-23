    $(document).ready(function() {
        $.getJSON('../../../api/Account/GetUserInfo')
        	.done(function (data) {
                if (data === null) {
                    return;
                }else {
                    $("#titlename").html(data.name);
                }
	    });

        $("#signout").click(function(){
            $.getJSON('../../../api/Account/SignOut')
                .done(function (data) {
                    console.log(data);

            });
            window.location.href = "../signin.html";
        });

    });