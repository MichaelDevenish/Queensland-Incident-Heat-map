function querya() {
    var querys = $('#search').val();
    if (!isNaN(querys) && querys != "" && querys.length === 4) {
        $.post('/search.aspx', { location: querys }, function (information) {
            $("#Label1").text(information.name);
            var locations = information.data;
            LoadMap(querys, locations);
        });
    } else {
        //show error
    }
}


function LoadMap(postcode,locations) {
    geocoder = new google.maps.Geocoder();
    var loco = locations;
    geocoder.geocode({ 'address': postcode, 'region': 'AU' }, function (results, status) {
        if (status == 'OK') {
            var a = results;
            bounds = new google.maps.LatLngBounds();
            map = new google.maps.Map(document.getElementById('map'), {
                zoom: 10,
                center: results[0].geometry.location ,
                mapTypeId: 'roadmap'
            });
            var heatmapData = [];
            for (var i = 0; i < loco.length; i++) {
                var coords = loco[i];
                var latLng = new google.maps.LatLng(coords.Lat, coords.Lng);
                bounds.extend(latLng);
                heatmapData.push(latLng);
            }
            var heatmap = new google.maps.visualization.HeatmapLayer({
                data: heatmapData,
                map: map
            });
            map.fitBounds(bounds);
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}
