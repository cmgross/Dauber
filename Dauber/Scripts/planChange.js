function resetFields() {
    //$("#").val("");
}

function userCardCheck() {
    var hasPaymentMethod = $("#HasPaymentMethod").val();
    if (hasPaymentMethod === "False") {
        $("#dPaymentInfo").css("display", "");
    } else {
        $("#dPaymentInfo").css("display", "none");
    }
}

$(document).ready(function () {
    userCardCheck();
    $("#PlanId").change(function () {
        var planId = $("#PlanId").val();
        var url = "/MyAccount/GetPlan";
        var currentClients = $("#NumberOfClients").val();
        $.ajax({
            contentType: "application/html; charset=utf-8",
            dataType: "json",
            type: "GET",
            cache: true,
            url: url,
            data: { planId: planId },
            beforeSend: function () {
                $.blockUI({
                    message: "Loading...",
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#000',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    }
                });
            },
            success: function (data) {
                var json = jQuery.parseJSON(data);
                
                if (json.MaxClients < currentClients) {
                    $("#btnSave").prop("disabled", true);
                    swal("Oops...", "You have too many clients to switch to this plan!", "error");
                    return;
                }

                $("#PlanCost").val(json.Cost);
                $("#PlanCost").format()
                $("#PlanMax").val(json.MaxClients);
                $("#btnSave").prop("disabled", false);
            },
            complete: function () {
                $.unblockUI();
            },
            error: function (xhr, status) {
                alert(status);
            }
        });

    });
});