function getCoordinates(address) {
    //var latlng = new google.maps.LatLng(lat, lng);
    var geocoder = geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == "OK") {
            if (results[0]) {
                var latLng = results[0].geometry.location;
                sendRequest(latLng);
            }
        } else {
            toastr.error("Geocode error : " + status);            
        } 
    });
}