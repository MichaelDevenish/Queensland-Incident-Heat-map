﻿var map = null;

function querya() {
    var querys = $('#search').val();
    var minYear = $('#BottomYear').val();
    var maxYear = $('#topYear').val();
    if (!isNaN(querys) && querys != "" && querys.length === 4) {
        $.post('/search.aspx', { location: querys, minYear: minYear, maxYear: maxYear}, function (information) {
            var locations = information.data;
            LoadMap(querys, locations);
        });
    } else {
        //show error
    }
}

function minSelected() {
    var valuemin = $('#BottomYear option:selected').text();
    var valuemax = $('#topYear option:selected').text();
    $('#topYear').empty();
    for (var i = valuemin; i <= 2017; i++) {
        $('#topYear').append($("<option></option>").attr("value", i).text(i));
    }
    if (valuemax >= valuemin) {
        $('#topYear').val(valuemax);
    } else {
        $('#topYear').val(valuemin);
    }
}

function maxSelected() {
    var valuemin = $('#BottomYear option:selected').text();
    var valuemax = $('#topYear option:selected').text();
    $('#BottomYear').empty();
    for (var i = 2001; i <= valuemax; i++) {
        $('#BottomYear').append($("<option></option>").attr("value", i).text(i));
    }
    if (valuemin <= valuemax) {
        $('#BottomYear').val(valuemin);
    } else {
        $('#BottomYear').val(valuemax);
    }
}

function LoadMap(postcode,locations) {
    geocoder = new google.maps.Geocoder();
    var loco = locations;
    geocoder.geocode({ 'address': postcode, 'region': 'AU' }, function (results, status) {
        if (status == 'OK') {
            bounds = new google.maps.LatLngBounds();
            map.panTo(results[0].geometry.location);
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

function drawMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 2,
        center: { lat: 0, lng:0 },
        mapTypeId: 'roadmap'
    });
}