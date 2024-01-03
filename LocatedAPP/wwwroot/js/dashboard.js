// Base URL for API requests
var url_base = 'http://localhost:5291';

// Retrieve token from local storage
var valorArmazenado = localStorage.getItem('Token-Located');

// Mapbox access token
var ACCESS_TOKEN_MAPBOX = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

var token = localStorage.getItem('Token-Located');

// Headers for the API request
var headers = {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
};

// Função para gerar miniaturas do mapa
function generateMapThumbnail(container, routeColor, distance, averageSpeed) {
    // Limita o número de casas decimais a dois
    distance = parseFloat(distance).toFixed(2);
    // Limita a velocidade média a duas casas decimais
    averageSpeed = parseFloat(averageSpeed).toFixed(2);

    // Crie o contêiner no layout
    $('#grid-container').append('<div id="' + container + '" class="map-thumbnail col-md-4 col-ls-4 col-lg-4 col-ms-4">' +
        '<p style="color:' + routeColor + '">Distância: ' + distance + ' km</p>' +
        '<p style="color:' + routeColor + '">Velocidade Média: ' + averageSpeed + ' km/h</p></div>');
}

// Função para buscar targets
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

// Executar o código quando o DOM estiver totalmente carregado
$(document).ready(function () {
    if (valorArmazenado) {
        // Fetch targets and process the result
        GetTargets().then(function (result) {
            console.log("result: ", result);

            // Check if the result is an array and not empty
            if (Array.isArray(result.distances) && result.distances.length > 0) {
                result.distances.forEach(function (item, index) {
                    // Crie um ID de contêiner exclusivo para cada miniatura
                    var mapContainerId = 'map-container-' + index;

                    console.log("item: ", item)
                    // Gere a miniatura do mapa usando a função ajustada
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
