var roomcount = 1;
var adults = 0;
var date = "";
var WhereTo = "";
var origin = "";
var destination = "";
var TravelClass = "";
var currency = "";
var child = 0;
var children = 0;
var passengers = 0;
$(document).ready(function () {
    StepperStatus();
});

function FormChange(btn_name) {
    $(".btndowning").removeClass("btndowningactive");
    $(`[form-id="${btn_name}"]`).children().addClass("btndowningactive");
    $(".frontpageform").addClass("d-none");
    $(`[action-form-name="${btn_name}"]`).removeClass("d-none");
}

function StepperStatus() {
    var currentURL = window.location.href;
    //Flights, Hotels and Transport stepper
    if (currentURL.includes("MakkahHotel") || currentURL.includes("MadinahHotel")) {
        $(".stepper-flights-bar").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-hotels-circle").removeClass("stepper-gray").addClass("bg-dark");
    }
    else if (currentURL.includes("Transport")) {
        $(".stepper-flights-bar").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-hotels-circle").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-hotels-bar").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-Extras-circle").removeClass("stepper-gray").addClass("bg-dark");
    }
    //Itinerary ,Details and payment
    else if (currentURL.includes("Details")) {
        $(".stepper-Itinerary-bar").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-Details-circle").removeClass("stepper-gray").addClass("bg-dark");
    }
    else if (currentURL.includes("Payment")) {
        $(".stepper-Itinerary-bar").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-Details-circle").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-Details-bar").removeClass("stepper-gray").addClass("bg-dark");
        $(".stepper-Payment-circle").removeClass("stepper-gray").addClass("bg-dark");
    }
}


$(document).ready(function () {
    var currentURL = window.location.href;
    var url = new URL(currentURL);
    var section = url.searchParams.get("section");
    if (section) {
        FormChange(section);
    }
    $(document).on("click", ".btndowning", function () {
        var btn_name = $(this).parent().attr("form-id");
        FormChange(btn_name);
    });
    $('#toggleButton').click(function () {
        var slidePanel = $('#slidePanel');
        var overlay = $('#overlay');
        var buttonText = $('#buttonText');
        slidePanel.toggleClass('visible');
        overlay.toggleClass('visible');
        if (slidePanel.hasClass('visible')) {
            buttonText.text('Close');
        } else {
            buttonText.text('Your Selection');
        }
    });

    $('#overlay').click(function () {
        var slidePanel = $('#slidePanel');
        var buttonText = $('#buttonText');
        slidePanel.removeClass('visible');
        $(this).removeClass('visible');
        buttonText.text('Your Selection');
        $('body').css('overflow', 'auto');
    });

    $(document).on("click", `[data-edit="edit-data"]`, function () {
        $("#hotelModal").modal("show");
    })
    $(document).on("click", `[data-modal="editdatamodal"]`, function () {
        $("#roomModal").modal("show");
    })

    $(document).on("click", "#SignInbtn", function () {
        $("#btnSignInSubmit").click();
    });
    $(document).on("click", ".nav_btn", function () {
        var section = $(this).attr("data-action");
        window.location.href = `/Home/Index?section=${section}`;
    });
    $(document).on("click", "#SignUpbtn", function () {
        $("#btnSignUpSubmit").click();
    });

    $(document).on("click", `[data-name="multi_check"]`, function () {
        $("#multicity_fields").removeClass("d-none");
    });

    $(document).on("click", `[data-chebox="return"]`, function () {
        $("#multicity_fields").addClass("d-none");
        $(`[data-id="selected-date"]`).attr("placeholder", "Return");
    })

    $(document).on("click", `[data-check="one_way"]`, function () {
        $("#multicity_fields").addClass("d-none");
        $(`[data-id="selected-date"]`).attr("placeholder", "Departure");
    })

    $(document).on("click", "#seenbutton", function () {
        var PasswordInput = $("#Password");

        if (PasswordInput.attr("type") == "password") {
            PasswordInput.attr("type", "text");
            $("#seenbutton").children("i").removeClass("fa-eye").addClass("fa-eye-slash");
        }
        else {
            PasswordInput.attr("type", "password");
            $("#seenbutton").children("i").removeClass("fa-eye-slash").addClass("fa-eye");
        }
    });

    $(document).on("click", "#showmoredropdowns", function () {
        roomcount++;
        const removeRoombtn = `
                <div class="mt-2" data-btn="remove-room">
                    <span class="RightArrowbtn ms-3">
                     <i class="fa fa-minus text-light"></i>
                    </span>
                </div>`;
        const newDropdown = $(`
                <div class="w-100 room_details d-flex flex-row">
                    <p class="me-3 mt-3 fw-bold room-number">Room ${roomcount}</p>
                    <div class="w-25 mt-3">
                        <select class="dropdown-adults w-100">
                            <option value="0">0</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                        </select>
                    </div>
                    <div class="w-25 mt-3 ms-3">
                        <select class="w-100 dropdown-child">
                            <option value="0">0</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                        </select>
                    </div>
                    ${removeRoombtn}
                </div>`);

        $("#dropdowns").append(newDropdown);
        updateRoomLabels();
    });

    function updateRoomLabels() {
        $(".room-number").each(function (index) {
            $(this).text(`Room ${index + 1}`);
        });
    }

    $(document).on("click", '[data-btn="remove-room"]', function () {
        $(this).parent(".room_details").remove();
        updateRoomLabels();
    });

    $(document).on("change", ".dropdown-adults", function () {
        adults = adults + parseInt($(this).val());
    });

    $(document).on("change", ".passenger-dropdown", function () {
        debugger;
        passengers = passengers + parseInt($(this).val());
    });

    $(document).on("change", ".child-passeng-dropdown", function () {
        debugger;
        child = child + parseInt($(this).val());
    });

    $(document).on("change", ".child-dropdown", function () {
        children = children + parseInt($(this).val());
    });

    $(document).on("change", "#HotelsDepartureDate", function () {
        date = $(this).val();
    });
    $(document).on("keyup", "#HotelsWhereTo", function () {
        WhereTo = $(this).val();
    });
    $(document).on("click", "#BtnWhoisgoinOk", function () {
        roomcount = $(".room_details").length + 1;
        adults = 0;

        $(".dropdown-adults").each(function () {
            adults += parseInt($(this).val());
        });

        $(`[data-name="Who-is-going"]`).val(`${roomcount} Room, ${adults} Adults`);
    });


    $(document).on("click", "#BtnWhoisgoing", function () {
        debugger;
        $(`[data-passenger="Who-is-going"]`).val(`${passengers} Adults, ${child} Childs`);
        passengers = 0;
        child = 0;
    });

    $(document).on("click", "#BtnHotelsSearch", function () {
        if (WhereTo == "Makkah")
            window.location.href = `/Hotels/MakkahHotel?query=Makkah&CheckinDate=${date}&CheckOutDate=${date}&adults=${adults}&sort=PRICE_LOW_TO_HIGH&regionId=6118146`;
        else if (WhereTo == "Madinah")
            window.location.href = `/Hotels/MakkahHotel?query=Madinah&CheckinDate=${date}&CheckOutDate=${date}&adults=${adults}&sort=PRICE_LOW_TO_HIGH&regionId=602705`;

    });
    $(document).on("click", `[form-id="Umrah-booking"]`, function () {
        sessionStorage.setItem("Flights", null);
        origin = $(`[input-id="Umrah-booking-From"]`).val();
        destination = $(`[input-id="Umrah-booking-WhichCity"]`).find(":selected").val();
        date = $(`[input-id="Umrah-booking-When"]`).val();
        TravelClass = $(`[input-id="Umrah-booking-TravelClass"]`).val();

        window.location.href = `/Flights/Index`;
        sessionStorage.setItem('origin', origin);
        sessionStorage.setItem('destination', destination);
        sessionStorage.setItem('DepartureDate', date);
        sessionStorage.setItem('adults', adults);
        sessionStorage.setItem('child', child);
        sessionStorage.setItem('currency', "USD");
        sessionStorage.setItem('TravelClass', TravelClass);
    });

});

$(document).ready(function () {

    $(`[data-id="selected-date"]`).flatpickr({
        mode: "range",
        dateFormat: "M j, Y",
        showMonths: 2,
        minDate: "today",

        onChange: function (selectedDates, dateStr, instance) {

            if (selectedDates.length > 1) {
                debugger;

                var startDate = selectedDates[0];
                var endDate = selectedDates[selectedDates.length - 1];

                var differenceInTime = endDate.getTime() - startDate.getTime();
                var differenceInDays = differenceInTime / (1000 * 3600 * 24) + 1;
                $(".selectedDaysCount").text("Selected Days: " + differenceInDays);
                console.log($(".selectedDaysCount").text());

            } else {
                $("#selectedDaysCount").text("Selected Days: 0");
            }
        }
    });
    $(".flatpickr-calendar").append('<p class="mb-2 selectedDaysCount fw-bold"></p>');
});

Dropzone.options.myDropzone = {
    paramName: "file",
    dictDefaultMessage: "Drag files here or click to upload",
    init: function () {
        this.on("success", function (file, response) {
            alert("File uploaded successfully");
        });
        this.on("error", function (file, response) {
            alert("Error uploading file");
        });
    }
};


function initMap() {
    // Check if google.maps is available
    if (!google.maps) {
        console.error("Google Maps JavaScript API is not loaded.");
        return;
    }

    const map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 0, lng: 0 },
        zoom: 2,
    });

    const input = document.getElementById("pac-input");

    if (!input) {
        console.error("Input element #pac-input not found.");
        return;
    }

    const searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    let markers = [];

    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    searchBox.addListener('places_changed', function () {
        const places = searchBox.getPlaces();

        if (places.length === 0) {
            return;
        }

        markers.forEach(marker => {
            marker.setMap(null);
        });
        markers = [];

        const bounds = new google.maps.LatLngBounds();

        places.forEach(place => {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            const icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25),
            };

            markers.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location,
            }));

            document.getElementById("latitude").value = place.geometry.location.lat();
            document.getElementById("longitude").value = place.geometry.location.lng();

            if (place.geometry.viewport) {
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });

    map.addListener('click', function (event) {
        const clickedLocation = event.latLng;
        document.getElementById("latitude").value = clickedLocation.lat();
        document.getElementById("longitude").value = clickedLocation.lng();

        if (markers.length > 0) {
            markers[0].setPosition(clickedLocation);
        } else {
            markers.push(new google.maps.Marker({
                position: clickedLocation,
                map: map,
            }));
        }
    });
}



$(document).ready(function () {
    initMap();
});


