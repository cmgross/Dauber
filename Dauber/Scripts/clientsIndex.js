$("#clientsAdd").submit(function (event) {
    event.preventDefault();

    if (!$("#clientsAdd").valid()) {
        return;
    }

    var url = "/Clients/GetClientMatches";
    var clientUserName = $("#ClientUserName").val();
    var userId = $("#UserId").val();
    var fitocracyUserName = $("#FitocracyUserName").val();
    var clientAction = "Add";

    $.ajax({
        contentType: "application/html; charset=utf-8",
        dataType: "json",
        type: "GET",
        cache: true,
        url: url,
        data: { clientUserName: clientUserName, userId: userId, clientAction: clientAction, fitocracyUserName: fitocracyUserName },
        beforeSend: function () {
            $.blockUI({
                message: "Checking MFP Diary status...",
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

            if (!json.IsPartner && !json.IsAdmin && ((json.CurrentClientsCount + 1) > json.MaxClientsForPlan)) {
                swal("Oops...", "You've exceeded your max number of clients. Please upgrade your plan to add more!", "error");
                return;
            }

            if (!json.IsValidFitocracyUser) {
                swal("Oops...", "This is not a valid Fitocracy username.", "error");
            }

            if (json.IsAvailable && json.IsPublic && json.IsValidFitocracyUser) {
                $("#FitocracyId").val(json.FitocracyUserId);
                $("#clientsAdd")[0].submit();
            }

            if (!json.IsAvailable) {
                swal("Oops...", "This MFP user is already assigned to a coach.", "error");
                return;
            }

            if (!json.IsPublic) {
                swal("Oops...", "This MFP user's diary is not currently public.", "error");
                return;
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

$("#clientsEdit").submit(function (event) {
    event.preventDefault();

    if (!$("#clientsEdit").valid()) {
        return;
    }

    var url = "/Clients/GetClientMatches";
    var clientUserName = $("#ClientUserName").val();
    var userId = $("#UserId").val();
    var fitocracyUserName = $("#FitocracyUserName").val();
    var clientAction = "Edit";

    $.ajax({
        contentType: "application/html; charset=utf-8",
        dataType: "json",
        type: "GET",
        cache: true,
        url: url,
        data: { clientUserName: clientUserName, userId: userId, clientAction: clientAction, fitocracyUserName: fitocracyUserName },
        beforeSend: function () {
            $.blockUI({
                message: "Checking MFP Diary status...",
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

            if (json.IsAvailable && json.IsPublic && json.IsValidFitocracyUser) {
                $("#FitocracyId").val(json.FitocracyUserId);
                $("#clientsEdit")[0].submit();
            }

            if (!json.IsValidFitocracyUser) {
                swal("Oops...", "This is not a valid Fitocracy username.", "error");
            }

            if (!json.IsAvailable) { //Assigned to another coach
                swal("Oops...", "This MFP user is already assigned to another coach.", "error");
                return;
            }

            if (!json.IsPublic) {
                swal("Oops...", "This MFP user's diary is not currently public.", "error");
                return;
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

$('.deleteClientForm').click(function (e) {
    e.preventDefault();
});

function DeleteConfirmation(id) {
    swal({
        title: "Are you sure?",
        text: "Are you sure that you want to delete this client?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete!",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: true
    },
    function(isConfirm) {
         if (isConfirm) {
             $("#" + id).submit();
         }
    });
}