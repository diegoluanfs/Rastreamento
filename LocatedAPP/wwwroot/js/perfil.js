$(document).ready(function () {
    var url_base = 'http://localhost:5291';



    //#region login

    var token = localStorage.getItem('Token-Located');
    var headers = {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    };

    $.ajax({
        url: url_base + '/api/persons',
        type: 'GET',
        headers: headers,
        success: function (result) {
            $('#idField').text(result.data[0].id);
            $('#usernameField').text(result.data[0].username);
            $('#emailField').text(result.data[0].email);
        },
        error: function (error) {
            console.log(error);
            Swal.fire({
                position: "top-end",
                icon: "error",
                title: error.responseJSON.message,
                showConfirmButton: false,
                timer: 2500
            });
        }
    });
    //#endregion



});
