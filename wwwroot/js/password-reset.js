let EMAIL_FORM_SELECTOR = '.input-group';

let App = window.App;
let FormHandler = App.FormHandler;
let EMAIL_FORM = new FormHandler(EMAIL_FORM_SELECTOR);

$(function () {
    EMAIL_FORM.addSubmitHandler(sendEmail);
});

function sendEmail(data) {
    $.ajax({
        type: 'POST',
        url: '/PasswordReset/SendEmail',
        data: {email: data.email},
        success: function(data) {
            if (data) {
                alert('Successfully sent email');
            } else {
                alert('Email not recognized');
            }
        }
    })
}
