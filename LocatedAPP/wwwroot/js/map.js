
// Base URL for API requests
var url_base = 'http://localhost:5291';

// Retrieve token from local storage
var valorArmazenado = localStorage.getItem('Token-Located');

// Array to store location data
var locationsMax = [];

// Mapbox access token
ACCESS_TOKEN_MAPBOX = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

// Inicializa os marcadores e as posições
const markers = [];

var routeCoordinatesArray = []; // Nova variável para armazenar os dados das coordenadas das rotas
var positionsDescription = [];

if (valorArmazenado) {
    // Set Mapbox access token
    mapboxgl.accessToken = ACCESS_TOKEN_MAPBOX;

    // Create Mapbox map
    const map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v12',
        center: [-53.81106, -29.69370],
        zoom: 12,
        interactive: true
    });

    // Fetch targets and process the result
    GetTargets().then(function (result) {
        // Check if the result is an array and not empty
        if (Array.isArray(result) && result.length > 0) {
            result.forEach(function (item) {
                // Create start location object
                var locationStart = {
                    coordinates: [item.latitudeStart, item.longitudeStart],
                    name: 'Ponto ' + item.id,
                    color: item.color
                };

                // Create end location object
                var locationEnd = {
                    coordinates: [item.latitudeEnd, item.longitudeEnd],
                    name: 'Ponto ' + item.id,
                    color: item.color
                };

                var info = {
                    idTarget: item.id,
                    color: item.color
                }

                positionsDescription.push(info);

                // Add locations to the array
                locationsMax.push(locationStart);
                locationsMax.push(locationEnd);
            });
        } else {
            console.log('A lista está vazia ou não é um array.');
        }

        // After the map loads
        map.on('load', () => {
            // Add markers to the map
            for (let i = 0; i < locationsMax.length; i++) {
                const location = locationsMax[i];

                new mapboxgl.Marker({ color: location.color })
                    .setLngLat([location.coordinates[0], location.coordinates[1]])
                    .setPopup(new mapboxgl.Popup().setHTML(`<h3>${location.name}</h3>`))
                    .addTo(map);
            }

            // Add routes and directions to the map
            for (let i = 0; i < locationsMax.length; i += 2) {
                const startLocation = locationsMax[i];
                const endLocation = locationsMax[i + 1];

                const coordinates = [startLocation.coordinates, endLocation.coordinates];

                // Request optimized route directions from Mapbox
                axios.get(`https://api.mapbox.com/directions/v5/mapbox/driving/${coordinates[0][0]},${coordinates[0][1]};${coordinates[1][0]},${coordinates[1][1]}`, {
                    params: {
                        steps: true,
                        geometries: 'geojson',
                        overview: 'full',
                        language: 'en',
                        access_token: ACCESS_TOKEN_MAPBOX
                    }
                }).then(response => {

                    // Armazene os dados das coordenadas das rotas
                    routeCoordinatesArray.push(response.data.routes[0].geometry.coordinates);

                    // Process the route response
                    const route = response.data.routes[0];
                    const routeCoordinates = route.geometry.coordinates;

                    // Add route source to the map
                    map.addSource(`route-${i}`, {
                        'type': 'geojson',
                        'data': {
                            'type': 'Feature',
                            'properties': {},
                            'geometry': {
                                'type': 'LineString',
                                'coordinates': routeCoordinates
                            }
                        }
                    });

                    // Add route layer to the map
                    map.addLayer({
                        'id': `route-${i}`,
                        'type': 'line',
                        'source': `route-${i}`,
                        'layout': {
                            'line-join': 'round',
                            'line-cap': 'round'
                        },
                        'paint': {
                            'line-color': startLocation.color,
                            'line-width': 2
                        }
                    });

                    // Add route steps source to the map
                    map.addSource(`route-steps-${i}`, {
                        'type': 'geojson',
                        'data': {
                            'type': 'Feature',
                            'properties': {},
                            'geometry': {
                                'type': 'LineString',
                                'coordinates': routeCoordinates
                            }
                        }
                    });

                    // Add route steps layer to the map
                    map.addLayer({
                        'id': `route-steps-${i}`,
                        'type': 'line',
                        'source': `route-steps-${i}`,
                        'layout': {
                            'line-join': 'round',
                            'line-cap': 'round'
                        },
                        'paint': {
                            'line-color': startLocation.color,
                            'line-width': 2
                        }
                    });

                    // Adiciona marcadores vermelhos nos primeiros pontos de cada vetor
                    routeCoordinatesArray.forEach((coordinates, index) => {
                        if (coordinates.length > 0) {
                            const firstPoint = coordinates[0];
                            const marker = new mapboxgl.Marker({ color: 'red' }) // Defina a cor como vermelha
                                .setLngLat(firstPoint)
                                .addTo(map);

                            markers.push(marker)

                            // Adicione um popup se desejar
                            const popup = new mapboxgl.Popup().setHTML(`<h3>Marker ${index}</h3>`);
                            marker.setPopup(popup);
                        }
                    });
                }).catch(error => {
                    console.error("error: ", error);
                });
            }
        });
    }).catch(function (error) {
        console.error("error: ", error);
    });
} else {
    console.log("não existe token")
}

// Function to fetch targets
function GetTargets() {
    return new Promise(function (resolve, reject) {
        // Retrieve token from local storage
        var token = localStorage.getItem('Token-Located');

        // Headers for the API request
        var headers = {
            'Authorization': 'Bearer ' + token,
            'Content-Type': 'application/json'
        };

        // AJAX request to fetch targets
        $.ajax({
            url: url_base + '/api/targetstomap',
            type: 'GET',
            headers: headers,
            success: function (result) {
                resolve(result);
            },
            error: function (error) {

                window.location.href = "./login.html";

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

const markerPositions = [];
const routesPositions = [];

// Define o intervalo (em milissegundos)
const intervalo = 100; // 5 segundos

function markersMoving() {
    const numberOfRoutes = routeCoordinatesArray.length;

    // Verifica se numberOfRoutes é menor ou igual a 0
    if (numberOfRoutes <= 0) {
        // Faça o que você precisa fazer enquanto numberOfRoutes é menor ou igual a 0
    } else {
        for (let i = 0; i < numberOfRoutes; i++) {
            markerPositions.push(i);
        }
        clearInterval(intervalo);
    }
}

// Inicia o intervalo apenas se numberOfRoutes for menor ou igual a 0
if (routeCoordinatesArray.length <= 0) {
    setInterval(markersMoving, intervalo);
}

let vetorPartialPositions = []
const routeLengthTotal = []
const positionsFinished = []

function updateMarkerPositions() {
    const numberOfRoutes = routeCoordinatesArray.length;

    for (let i = 0; i < numberOfRoutes; i++) {
        const route = routeCoordinatesArray[i];

        if (route.length > 0) {

            let partial = []

            if (positionsFinished.includes(i)) {
                if (positionsFinished.length == numberOfRoutes) {
                    clearInterval(idIntervalo);
                }
            } else {

                const currentPointIndex = markerPositions[i];
                const nextIndex = (currentPointIndex + 1) % route.length;

                // Obtém as coordenadas da rota específica
                const coordinates = route[nextIndex];

                const lngLat = new mapboxgl.LngLat(coordinates[0], coordinates[1]);

                partial = {
                    id: i,
                    latitude: coordinates[1],
                    longitude: coordinates[0]
                }

                // Verifica se o marcador correspondente existe antes de tentar atualizar
                if (markers[i]) {
                    // Atualiza a posição do marcador
                    markers[i].setLngLat(lngLat);

                    // Atualiza o índice da próxima posição
                    markerPositions[i] = nextIndex;

                    // Verifica se é a última posição
                    if (nextIndex === route.length - 1) {
                        positionsFinished.push(i);
                    }
                } else {
                    console.error('Marcador não definido para o índice ' + i);
                }

                vetorPartialPositions.push(partial);

                SaveData(partial, positionsDescription);
            }
        }
    }
}

function SaveData(partial, positionsDescription) {

    var token = localStorage.getItem('Token-Located');
    var headers = {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    };

    var vetorReq = {
        "idTarget": positionsDescription[partial.id].idTarget,
        "color": positionsDescription[partial.id].color,
        "id": partial.id,
        "latitude": partial.latitude,
        "longitude": partial.longitude
    }

    $.ajax({
        url: url_base + '/api/route',
        type: 'POST',
        headers: headers,
        contentType: 'application/json',
        data: JSON.stringify(vetorReq),
        success: function (result) {
            //console.log("result: ", result);
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


// Inicia a execução da função em intervalos regulares
const idIntervalo = setInterval(updateMarkerPositions, intervalo);