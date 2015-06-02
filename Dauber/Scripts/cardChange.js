//cardInfo
//updateCard instead of updatePlan
// @Html.Hidden("Token")
//                @Html.HiddenFor(m => m.CustomerId)
//                @Html.HiddenFor(m => m.HasPaymentMethod)
//                @Html.HiddenFor(m => m.CardId)
//                @Html.HiddenFor(m => m.CoachId)

//btnSave

$(document).ready(function () {
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
        var $form = $("#updateCard");

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