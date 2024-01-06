
var url_base = 'http://localhost:5291';

var valorArmazenado = localStorage.getItem('Token-Located');

var ACCESS_TOKEN_MAPBOX = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

var token = localStorage.getItem('Token-Located');

var headers = {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
};

function generateMapThumbnail(container, routeColor, distance, averageSpeed) {
    distance = parseFloat(distance).toFixed(2);
    averageSpeed = parseFloat(averageSpeed).toFixed(2);

    $('#grid-container').append('<div id="' + container + '" class="map-thumbnail col-md-4 col-ls-4 col-lg-4 col-ms-4">' +
        '<p style="color:' + routeColor + '">Distância: ' + distance + ' km</p>' +
        '<p style="color:' + routeColor + '">Velocidade Média: ' + averageSpeed + ' km/h</p></div>');
}

function GetTargets() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url_base + '/api/targets/dashboard',
            type: 'GET',
            headers: headers,
            success: function (result) {
                resolve(result);
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
}

$(document).ready(function () {
    if (valorArmazenado) {
        GetTargets().then(function (result) {
            console.log("result: ", result);

            if (Array.isArray(result.distances) && result.distances.length > 0) {
                result.distances.forEach(function (item, index) {
                    var mapContainerId = 'map-container-' + index;

                    console.log("item: ", item)
                    generateMapThumbnail(mapContainerId, item.color, item.distance, item.averageSpeed);
                });
            } else {
                console.log('A lista está vazia ou não é um array.');
            }
        }).catch(function (error) {
            console.error("error: ", error);
        });
    } else {
        console.log("não existe token");
    }
});
