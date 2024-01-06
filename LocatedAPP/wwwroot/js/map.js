
var url_base = 'http://localhost:5291';

var valorArmazenado = localStorage.getItem('Token-Located');

var locationsMax = [];

ACCESS_TOKEN_MAPBOX = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

const markers = [];

var routeCoordinatesArray = [];
var positionsDescription = [];

if (valorArmazenado) {
    mapboxgl.accessToken = ACCESS_TOKEN_MAPBOX;

    const map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v12',
        center: [-53.81106, -29.69370],
        zoom: 12,
        interactive: true
    });

    GetTargets().then(function (result) {
        if (Array.isArray(result) && result.length > 0) {
            result.forEach(function (item) {
                var locationStart = {
                    coordinates: [item.latitudeStart, item.longitudeStart],
                    name: 'Ponto ' + item.id,
                    color: item.color
                };

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

                locationsMax.push(locationStart);
                locationsMax.push(locationEnd);
            });
        } else {
            console.log('A lista está vazia ou não é um array.');
        }

        map.on('load', () => {
            for (let i = 0; i < locationsMax.length; i++) {
                const location = locationsMax[i];

                new mapboxgl.Marker({ color: location.color })
                    .setLngLat([location.coordinates[0], location.coordinates[1]])
                    .setPopup(new mapboxgl.Popup().setHTML(`<h3>${location.name}</h3>`))
                    .addTo(map);
            }

            for (let i = 0; i < locationsMax.length; i += 2) {
                const startLocation = locationsMax[i];
                const endLocation = locationsMax[i + 1];

                const coordinates = [startLocation.coordinates, endLocation.coordinates];

                axios.get(`https://api.mapbox.com/directions/v5/mapbox/driving/${coordinates[0][0]},${coordinates[0][1]};${coordinates[1][0]},${coordinates[1][1]}`, {
                    params: {
                        steps: true,
                        geometries: 'geojson',
                        overview: 'full',
                        language: 'en',
                        access_token: ACCESS_TOKEN_MAPBOX
                    }
                }).then(response => {

                    routeCoordinatesArray.push(response.data.routes[0].geometry.coordinates);

                    const route = response.data.routes[0];
                    const routeCoordinates = route.geometry.coordinates;

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

                    routeCoordinatesArray.forEach((coordinates, index) => {
                        if (coordinates.length > 0) {
                            const firstPoint = coordinates[0];
                            const marker = new mapboxgl.Marker({ color: 'red' })
                                .setLngLat(firstPoint)
                                .addTo(map);

                            markers.push(marker)

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

function GetTargets() {
    return new Promise(function (resolve, reject) {
        var token = localStorage.getItem('Token-Located');

        var headers = {
            'Authorization': 'Bearer ' + token,
            'Content-Type': 'application/json'
        };

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

const intervalo = 100;

function markersMoving() {
    const numberOfRoutes = routeCoordinatesArray.length;

    if (numberOfRoutes <= 0) {
    } else {
        for (let i = 0; i < numberOfRoutes; i++) {
            markerPositions.push(i);
        }
        clearInterval(intervalo);
    }
}

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

                const coordinates = route[nextIndex];

                const lngLat = new mapboxgl.LngLat(coordinates[0], coordinates[1]);

                partial = {
                    id: i,
                    latitude: coordinates[1],
                    longitude: coordinates[0]
                }

                if (markers[i]) {
                    markers[i].setLngLat(lngLat);

                    markerPositions[i] = nextIndex;

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
            console.log("result: ", result);
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

const idIntervalo = setInterval(updateMarkerPositions, intervalo);