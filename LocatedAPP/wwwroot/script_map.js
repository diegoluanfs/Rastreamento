mapboxgl.accessToken = 'pk.eyJ1IjoiZGllZ29sdWFuZnMiLCJhIjoiY2xxZHJnOWE0MGV6MzJpcGxtdnJwY25pYyJ9.V9llMCoz-QmkNpSXxAgj8Q';

var map = L.map('map').setView([14.0860746, 100.608406], 6);

var osm = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
});
osm.addTo(map);

var marker1, marker2;

function startTracking() {
    if (!navigator.geolocation) {
        console.log("Your browser doesn't support geolocation feature!");
    } else {
        setInterval(() => {
            navigator.geolocation.getCurrentPosition(getPosition);
        }, 5000);
    }
}

function getPosition(position) {
    var lat = position.coords.latitude;
    var long = position.coords.longitude;
    var accuracy = position.coords.accuracy;

    if (!marker1) {
        marker1 = L.marker([lat, long]).addTo(map);
    } else if (!marker2) {
        marker2 = L.marker([lat, long]).addTo(map);
        var line = L.polyline([marker1.getLatLng(), marker2.getLatLng()], { color: 'blue' }).addTo(map);
        map.fitBounds(line.getBounds());
    } else {
        map.removeLayer(marker1);
        map.removeLayer(marker2);
        marker1 = L.marker([lat, long]).addTo(map);
        marker2 = null;
    }

    console.log("Your coordinate is: Lat: " + lat + " Long: " + long + " Accuracy: " + accuracy);
}
