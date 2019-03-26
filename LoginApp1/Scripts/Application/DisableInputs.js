
function DisableInputs(flag) {
    if (flag) {
        $(":button").prop("disabled", true);
        $(":input").prop("disabled", true);
        $(".form-horizontal").prop("disabled", true);
        $(".small_inp_box").prop("disabled", true);
        $("input:password").prop("disabled", true);
        $("#btn_submit").prop("disabled", true);
        $("#btn_submit").text("Processing", true);
    }
    else {
        $(":button").prop("disabled", false);
        $(":input").prop("disabled", false);
        $(".form-horizontal").prop("disabled", false);
        $(".small_inp_box").prop("disabled", false);
        $("input:password").prop("disabled", false);
        $("#btn_submit").prop("disabled", false);
        $("#btn_submit").text("Save Role");
    }
}
