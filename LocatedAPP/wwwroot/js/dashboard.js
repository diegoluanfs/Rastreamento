
var url_base = 'http://localhost:5291';

var token = localStorage.getItem('Token-Located');
var headers = {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
};

$(document).ready(function () {
    $.ajax({
        url: url_base + '/api/targets/dashboard',
        type: 'GET',
        headers: headers,
        success: function (result) {
            console.log("result: ", result);
        },
        error: function (error) {
            reject(error);
            Swal.fire({
                position: "top-end",
                icon: "error",
                title: error.responseJSON.message,
                showConfirmButton: false,
                timer: 2500
            });
        }
    });
});

