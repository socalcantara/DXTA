function BootsAlert(alertType, titleAlert, titleMessage, withButton, divElementName, btnName) {
        if (withButton != "T" && alertType != "danger") {
            setTimeout(function () {
                $(".alert").fadeTo("fast", 0).slideUp("fast", function () {
                    $(this).remove();
                    if (alertType == "success") {
                        $('.modal').modal('hide');
                    }
                });

            }, 2000);
        }

        if (withButton == "T") {
            if (divElementName != "" && btnName != "") {
                $("#" + divElementName + "").html("<div class='alert alert-" + alertType + "'><strong>" + titleAlert + "!</strong> " + titleMessage + "<div class='pull-right'><a class='btn btn-default pull-right btn-sm' data-dismiss='alert'>Cancel</a><a type='button' id='" + btnName + "' class='btn btn-default pull-right btn-sm' style='margin-right:3px'>Yes</a></div></div>");
            }
            else {
                console.log("put div element id and btn id");
            }
        }

        if (withButton != "T") {
            if (alertType == "success") {
                $("#" + divElementName + "").html("<div class='alert alert-" + alertType + "'><strong>" + titleAlert + "!</strong> " + titleMessage + "</div>");
            }
            else {
                $("#" + divElementName + "").html("<div class='alert alert-" + alertType + "'><strong>" + titleAlert + "!</strong> " + titleMessage + "</div>");
            }
        }
}
//Nested Modal
function BootsAlertSecondModal(alertType, titleAlert, titleMessage, withButton, divElementName, btnName, modalID) {
    if (withButton != "T" && alertType != "danger") {
        setTimeout(function () {
            $(".alert").fadeTo("fast", 0).slideUp("fast", function () {
                $(this).remove();
                if (alertType == "success") {
                    $("#" + modalID).modal('toggle');
                }
            });

        }, 2000);
    }

    if (withButton == "T") {
        if (divElementName != "" && btnName != "") {
            $("#" + divElementName + "").html("<div class='alert alert-" + alertType + "'><strong>" + titleAlert + "!</strong> " + titleMessage + "<div class='pull-right'><a class='btn btn-default pull-right btn-sm' data-dismiss='alert'>Cancel</a><a type='button' id='" + btnName + "' class='btn btn-default pull-right btn-sm' style='margin-right:3px'>Yes</a></div></div>");
        }
        else {
            console.log("put div element id and btn id");
        }
    }

    if (withButton != "T") {
        if (alertType == "success") {
                $("#" + divElementName + "").html("<div class='alert alert-" + alertType + "'><strong>" + titleAlert + "!</strong> " + titleMessage + "</div>");
        }
        else {
                $("#" + divElementName + "").html("<div class='alert alert-" + alertType + "'><strong>" + titleAlert + "!</strong> " + titleMessage + "</div>");
        }
    }
}