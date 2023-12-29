
var url_base = 'http://localhost:5291';

$('#termo').click(() => {
    const div = $('<div>').attr('style', `
                        position: absolute;
                        width: 65%;
                        height: 420px;
                        border-radius: 10px;
                        background: #fff;

                        color: #3d3d3d;

                        -webkit-box-shadow: 0px 0px 6px -1px #000000;
                        box-shadow: 0px 0px 6px -1px #000000;

                        display: flex;
                        flex-direction: column;
                        justify-content: space-evenly;
                        align-items: center;
                    `)

    $(div).addClass('termo')

    $(div).append(`Eu entendo e aceitos os termos impostos pela plataforma...`)

    $(div).append(
        $('<button>Ok</button>').click(() => {
            div.hide()
        })
    )

    $('body').append(div)
})

$('.button').on('click', function () {

    if ($('#email').val() != $('#cmail').val()) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Os emails devem ser iguais!",
            footer: '<a href="#">Why do I have this issue?</a>'
        });
    } else if ($('#password').val() != $('#cpassword').val()) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "As senhas devem ser iguais!",
            footer: '<a href="#">Why do I have this issue?</a>'
        });
    } else {
        console.log("chamado o ajax")

        var username = $('#username').val();
        var email = $('#email').val();
        var password = $('#password').val();

        var dataToSend = {
            username: username,
            email: email,
            password: password
        };

        console.log("dataToSend: ", dataToSend)

        $.ajax({
            url: url_base + '/api/person',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(dataToSend),
            success: function (result) {

                if (result.statusCode === 201) {
                    window.location.href = "./login.html";
                }
            },
            error: function (error) {
                Swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: error.responseJSON.message,
                    showConfirmButton: false,
                    timer: 2500
                });

            }
        });

    }
});