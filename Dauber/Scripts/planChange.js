function resetFields() {
    //$("#").val("");
}

function checkIfPaymentMethodNeeded() {
    var hasPaymentMethod = $("#HasPaymentMethod").val();
    if (hasPaymentMethod === "False") {
        $("#dPaymentInfo").css("display", ""); 
    } else {
        $("#dPaymentInfo").css("display", "none");
    }
}

$(document).ready(function () {
    checkIfPaymentMethodNeeded();

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

                $("#PlanCost").val("$" + json.Cost.toFixed(2));
                $("#PlanMax").val(json.MaxClients);
                var currentPlanId = $("#CurrentPlanId").val();
                if (currentPlanId != planId) {
                    $("#btnSave").prop("disabled", false);
                } else {
                    $("#btnSave").prop("disabled", true);
                }
            },
            complete: function () {
                $.unblockUI();
            },
            error: function (xhr, status) {
                alert(status);
            }
        });
    });

    $("#btnSave").on("click", function (e) {
        var hasPaymentMethod = $("#HasPaymentMethod").val();
        if (hasPaymentMethod === "True") return;
        $("#btnSave").prop("disabled", true);
        e.preventDefault();
        e.stopPropagation();
        var apiKey = $("#ApiKey").val();
        Stripe.setPublishableKey(apiKey);
        Stripe.card.createToken({
            number: $("#txtCardNumber").val(),
            cvc: $("#txtCvc").val(),
            exp_month: $("#txtExpiryMonth").val(),
            exp_year: $("#txtExpiryYear").val(),
            name: $("#UserName").val()
        }, stripeResponseHandler);
    });

    function stripeResponseHandler(status, response) {
        var $form = $("#updatePlan");

        if (response.error) {
            // Show the errors on the form
            alert(response.error.message);
            $("#btnSave").prop("disabled", false);
        } else {
            // response contains id and card, which contains additional card details
            var token = response.id;
            // Insert the token into the form so it gets submitted to the server
            $("#Token").val(token);
            // and submit
            $form.get(0).submit();
        }
    }
});