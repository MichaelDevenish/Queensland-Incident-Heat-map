var map = null;
var heatmap = null;

//Posts a request to /search.aspx with a postcode and min/max years and uses the 
//response to display a heat-map displaying all of the accidents in an area between
//the specified years
function queryDatabase() {
    //get the values
    var querys = $('#search').val();
    var minYear = $('#BottomYear').val();
    var maxYear = $('#topYear').val();
    //do error checking
    if (!isNaN(querys) && querys != "" && querys.length === 4) {
        //hide the warning that states "Must enter a valid Postcode"
        $("p#searchError").css('visibility', 'hidden');
        $("p#searchError").css('height', '0px');
        $("p#searchError").css('margin-bottom', '0px');
        //request the data from the server
        $.post('/search.aspx', { location: querys, minYear: minYear, maxYear: maxYear}, function (information) {
            var locations = information.data;
            LoadMap( locations);
        });
    } else {
        //show a warning stating "Must enter a valid Postcode" 
        $("p#searchError").css('visibility', 'visible');
        $("p#searchError").css('height', '24px');
        $("p#searchError").css('margin-bottom', '16px');
    }
}

//when the minimum year changes make sure the maximum year cannot be less than the minimum year
function minSelected() {
    //get values
    var valuemin = $('#BottomYear option:selected').text();
    var valuemax = $('#topYear option:selected').text();
    //clear top year's values
    $('#topYear').empty();
    //give it new values
    for (var i = valuemin; i <= 2017; i++) {
        $('#topYear').append($("<option></option>").attr("value", i).text(i));
    }
    //select the previous value, or if the value is now less than the min somehow make it the min
    if (valuemax >= valuemin) {
        $('#topYear').val(valuemax);
    } else {
        $('#topYear').val(valuemin);
    }
}

//when the maximum year changes make sure the minimum year cannot be less than the maximum year
function maxSelected() {
    //get values
    var valuemin = $('#BottomYear option:selected').text();
    var valuemax = $('#topYear option:selected').text();
    //clear min year's values
    $('#BottomYear').empty();
    //give it new values
    for (var i = 2001; i <= valuemax; i++) {
        $('#BottomYear').append($("<option></option>").attr("value", i).text(i));
    }
    //select the previous value, or if the value is now more than the max somehow make it the max
    if (valuemin <= valuemax) {
        $('#BottomYear').val(valuemin);
    } else {
        $('#BottomYear').val(valuemax);
    }
}

//Using a json list containing Lat/Lon data display a heat-map on the map and set the focus to it 
function LoadMap(locations) {
    //create bounds that are used to move the focus and zoom of the map
    bounds = new google.maps.LatLngBounds();
    //convert the coordinates to be compatible with the heat-map and extend the bounds to fit them
    var heatmapData = [];
    for (var i = 0; i < locations.length; i++) {
        var coords = locations[i];
        var latLng = new google.maps.LatLng(coords.Lat, coords.Lng);
        bounds.extend(latLng);
        heatmapData.push(latLng);
    }
    //clear the old heat-map if it exists
    if (heatmap !== null) {
        heatmap.setMap(null);
        heatmap.getData().j = [];
    }
    //create the new heat-map
    heatmap =  new google.maps.visualization.HeatmapLayer({
        data: heatmapData,
        map: map
    });
    //focus the map to the new heat-map
    map.fitBounds(bounds);
}

//when the application loads this is used to load the map at a basic position
function drawMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 2,
        center: { lat: 0, lng:0 },
        mapTypeId: 'roadmap'
    });
}