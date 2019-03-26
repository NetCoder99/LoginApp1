function ShowMessage(div_message, message, error) {
    if (error == true) {
        div_message.find("#img_common_success").hide();
        div_message.find("#img_common_warning").show();
    }
    if (error == false) {
        div_message.find("#img_common_success").show();
        div_message.find("#img_common_warning").hide();
    }
    if (error == null) {
        div_message.find("#img_common_success").hide();
        div_message.find("#img_common_warning").hide();
    }
    div_message.find("#lbl_common_message").text(message);
    div_message.show(0).delay(2000).hide(0);

}
