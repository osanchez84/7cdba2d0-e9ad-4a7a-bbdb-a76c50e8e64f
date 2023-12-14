// Write your Javascript code.
function hideLoading() {
    $('#loading').css('display', 'none');
}


function showLoading() {
    $('#loading').css('display', 'block');
}

var isValidPhone = function (phone) {
    var regex = /^[0-9]+$/;
    return regex.test(phone);
};

var isValidEmail = function (email) {
    var regex = /^\w+([.-_+]?\w+)*\w+([.-]?\w+)*(\.\w{2,10})+$/;
    return regex.test(email);
};