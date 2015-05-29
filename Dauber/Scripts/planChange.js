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
                if (currentPlanId !== planId) {
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
        $.blockUI({
            message: "Updating...",
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
        var hasPaymentMethod = $("#HasPaymentMethod").val();
        if (hasPaymentMethod === "True") return;
        $("#btnSave").prop("disabled", true);
        e.preventDefault();
        e.stopPropagation();
        var apiKey = $("#ApiKey").val();
        Stripe.setPublishableKey(apiKey);

        var cardNumber = $("#CardInfoViewModel_CreditCardNumber").val();
        var cvc = $("#CardInfoViewModel_Cvc").val();
        var expiryMonth = $("#CardInfoViewModel_ExpiryMonth").val();
        var expiryYear = $("#CardInfoViewModel_ExpiryYear").val();

        var validCard = Stripe.card.validateCardNumber(cardNumber);
        var validCvc = Stripe.card.validateCVC(cvc);
        var validExpiry = Stripe.card.validateExpiry(expiryMonth, expiryYear); 
       
        if (validCard === true && validCvc === true && validExpiry === true) { //and valid cvc, and valid expiry
            Stripe.card.createToken({
                number: cardNumber,
                cvc: cvc,
                exp_month: expiryMonth,
                exp_year: expiryYear,
                name: $("#UserName").val()
            }, stripeResponseHandler);
        } else {
            $.unblockUI();
            var errorMessage = "The following issues were found with your payment info:\n";

            if (validCard === false) {
                errorMessage += "Credit card invalid\n";
            }
            if (validCvc === false) {
                errorMessage += "CVC code invalid\n";
            }
            if (validExpiry === false) {
                errorMessage += "Expiration date invalid";
            }
            swal("Uh oh...", errorMessage, "error");
            $("#btnSave").prop("disabled", false);
            return;
        }
    });

    function stripeResponseHandler(status, response) {
        var $form = $("#updatePlan");

        if (response.error) {
            // Show the errors on the form
            swal("Uh oh...", response.error.message, "error");
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