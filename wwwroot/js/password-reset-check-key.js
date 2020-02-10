let CHANGE_PASS_FORM_SELECTOR = '#change-pass-form';

let App = window.App;
let FormHandler = App.FormHandler;
let PASS_CHANGE_FORM = new FormHandler(CHANGE_PASS_FORM_SELECTOR);

$(function () {
    PASS_CHANGE_FORM.addSubmitHandler(changePass);
});

function changePass(data) {
    if (data.password !== data.passwordCheck) {
        alert("Passwords don't match! Please try again.");
        return;
    } 
    
    $.ajax({
        type: 'POST',
        url: '/Auth/ChangePassword',
        data: {password: data.password, userId: data.userId},
        success: function(data) {
            if (data) {
                alert('Successfully updated password');
                window.location.replace("https://localhost:44361/");
            } else {
                alert('Something went wrong, try again.');
            }
        }
    });
}
