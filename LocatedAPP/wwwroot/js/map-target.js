
// Base URL for API requests
var url_base = 'http://localhost:5291';

// Retrieve token from local storage
var valorArmazenado = localStorage.getItem('Token-Located');

// Array to store location data
var locationsMax = [];

// Mapbox access token
ACCESS_TOKEN_MAPBOX = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

var token = localStorage.getItem('Token-Located');
var headers = {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
};

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

    getParamToURL();


    //#region pegar a rota pelo id

    // Função para obter parâmetros da URL
    function getParamToURL() {
        const urlSearchParams = new URLSearchParams(window.location.search);
        const parametros = Object.fromEntries(urlSearchParams.entries());

        // Verifica se o parâmetro existe
        const id = parametros.id;  // Use o valor do parâmetro 'id'
        console.log("id: ", id)
        $.ajax({
            url: url_base + '/api/route/' + id,
            type: 'GET',
            headers: headers,
            success: function (result) {
                console.log("result: ", result);
                if (result.statusCode == 200) {
                    GetPoints();
                }
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

    }

    //#endregion


} else {
    console.log("não existe token")
}
