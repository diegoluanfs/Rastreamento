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
            $('#idField').val(result.data[0].id);
            $('#usernameField').val(result.data[0].username);
            $('#emailField').val(result.data[0].email);
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

    //#region Update Perfil
    $('#save').on('click', function () {
        // Obtenha os valores dos campos
        var username = $('#usernameField').val();
        var email = $('#emailField').val();
        var id = $('#idField').val();

        // Construa o objeto de dados
        var data = {
            id: id,
            username: username,
            email: email
        };

        // Faça a chamada AJAX
        $.ajax({
            url: 'http://localhost:5291/api/persons',
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('Token-Located')
            },
            success: function (result) {
                console.log(result);

                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: result.message,
                    showConfirmButton: false,
                    timer: 2500
                });
            },
            error: function (error) {
                // Erro na chamada AJAX
                console.log(error);

                // Exibir mensagem de erro usando SweetAlert2
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: error.responseJSON.message,
                    showConfirmButton: false,
                    timer: 2500
                });
            }
        });
    });
    //#endregion


});
