function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(locCallback);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}

function locCallback(position) {
    console.log("Got users position");
    var content = {
        'Content-Type': 'application/json',
        'Longtuide': position.coords.longitude,
        'Latitude': position.coords.latitude ,
        'Accuracy': position.coords.accuracy ,
        'Speed': position.coords.speed

    }
    fetch('/api/employee/UpdateLoc', {
        method: 'POST', // or 'PUT'
        headers: content,
    })
        .then(response => response.json())
        .then(data => {
//            console.log('Success:', data);
            navigator.geolocation.getCurrentPosition(locCallback)
        })
        .catch((error) => {
            console.error('Error:', error);
            navigator.geolocation.getCurrentPosition(locCallback)
        });
}
getLocation();