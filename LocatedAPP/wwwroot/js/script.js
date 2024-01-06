$(document).ready(function () {
    var url_base = 'http://localhost:5291';
    //Implementar active nos links
    //var titleValue = document.title.trim();

    //#region Header
    var valorArmazenado = localStorage.getItem('Token-Located');

    if (!valorArmazenado) {
        var headerContent = `
        <a class="navbar-brand" href="#">Localize</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll" aria-controls="navbarScroll" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarScroll">
            <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">
                <li class="nav-item">
                    <a class="nav-link" aria-current="page" href="./index.html" id="index">In&iacute;cio</a>
                </li>
            </ul>
            <form class="d-flex">
                <div class="btn btn-success btn-login" id="login">Login</div>
            </form>
        </div>`;

        $("#header-verify").html(headerContent);
    } else {
        var name = localStorage.getItem('Username-Located');
        var headerContent = `
        <a class="navbar-brand" href="#">Localize</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll" aria-controls="navbarScroll" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarScroll">
            <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">
                <li class="nav-item">
                    <a class="nav-link" aria-current="page" href="./index.html">In&iacute;cio</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="./perfil.html">Perfil</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="./targets.html">Pontos</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" aria-current="page" href="./map.html">Mapa</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" aria-current="page" href="./dashboard.html">Dashboard</a>
                </li>
            </ul>

            <div class="my-2 mr-2" id="">Ol&aacute; `+ name + `</div>
            <form class="d-flex">
                <div class="btn btn-danger btn-logout" id="logout">Logout</div>
            </form>
        </div>`;

        $("#header-verify").html(headerContent);
    }
    //#endregion

    $('.btn-login').on('click', function () {
        window.location.href = "./login.html";
    });

    $('#recovery-password-access').on('click', function () {
        window.location.href = "./dev.html";
    });

    $('.btn-logout').on('click', function () {
        localStorage.setItem('Token-Located', '');
        localStorage.setItem('Username-Located', '');
        window.location.href = "./index.html";
    });

    //#region login
    $('#login-access').on('click', function () {
        var requestData = {
            "username": $('#username').val(),
            "password": $('#password').val()
        };

        $.ajax({
            url: url_base+ '/api/autenticar',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (result) {

                if (result.statusCode === 200 && result.token) {
                    localStorage.setItem('Token-Located', result.token);
                    localStorage.setItem('Username-Located', $('#username').val());

                    $('#username').val("")
                    $('#password').val("")

                    window.location.href = "./map.html";
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
    });
    //#endregion


});
