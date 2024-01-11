
var url_base = 'http://localhost:5291';

var valorArmazenado = localStorage.getItem('Token-Located');

var ACCESS_TOKEN_MAPBOX = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

var token = localStorage.getItem('Token-Located');

var headers = {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
};

function generateMapThumbnail(container, routeColor, distance, averageSpeed, idTarget) {
    distance = parseFloat(distance).toFixed(2);
    averageSpeed = parseFloat(averageSpeed).toFixed(2);

    var mapContainer = $('<div>')
        .addClass('map-thumbnail col-md-4 col-ls-4 col-lg-4 col-ms-4')
        .attr('id', container)
        .css({
            'text-align': 'center',
            'background-color': 'white',
            'font-weight': 'bold'
        });

    mapContainer.append(
        $('<p>').css('color', routeColor).text('Distância: ' + distance + ' km'),
        $('<p>').css('color', routeColor).text('Velocidade Média: ' + averageSpeed + ' km/h')
    );

    var mapSquare = $('<div>')
        .addClass('map-square')
        .css({
            'width': '80%',
            'height': '60%',
            'margin': '10px auto 0'
        });

    mapboxgl.accessToken = ACCESS_TOKEN_MAPBOX;
    var map = new mapboxgl.Map({
        container: mapSquare[0],
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [-53.76106, -29.74370],
        zoom: 10
    });

    mapContainer.append(mapSquare);
    $('#grid-container').append(mapContainer);

    GetRoutes(idTarget)
        .then(function (routes) {
            console.log("routes: ", routes);

            var coordinatesArray = routes.map(function (route) {
                return {
                    latitude: route.latitude,
                    longitude: route.longitude
                };
            });

            coordinatesArray.forEach(function (coordinates) {
                var marker = new mapboxgl.Marker({ color: routeColor })
                    .setLngLat([coordinates.longitude, coordinates.latitude])
                    .addTo(map);
            });
        })
        .catch(function (error) {
            console.error("Erro ao obter rotas: ", error);
        });
}

function GetRoutes(idTarget) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url_base + '/api/route/'+idTarget,
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
                    generateMapThumbnail(mapContainerId, item.color, item.distance, item.averageSpeed, item.idTarget);
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
