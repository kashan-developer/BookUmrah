var hotels = [];
var PriceSortingOrder;
var TotalHotels = 0;
$(document).ready(function () {
    GetHotelsList();
    GetValuesForFlights();
});
function GetHotelsList() {
    var sessionHotels = JSON.parse(sessionStorage.getItem('MakkahHotels'));
    if (!sessionHotels || sessionHotels.length==0) {
        TotalHotels = 0;
        $.ajax('/Hotels/GetAllHotels?query=Makkah&CheckinDate=2024-09-02&CheckOutDate=2024-09-02&adults=1&sort=PRICE_LOW_TO_HIGH&regionId=6118146', {
            success: function (data, xhr, status) {
                $("#MakkahHotelsCards").append(data.html);
                hotels = data.hotels;
                TotalHotels = data.hotels.length;
                $("#TotalMakkahHotels").text(TotalHotels);
                sessionStorage.setItem('MakkahHotels', JSON.stringify(hotels));
            }
        });
    }
    else {
        AppendMakkahHotels(sessionHotels);
    }
}
function sortHotelsByPrice(sessionHotels, sortOrder) {
    sessionHotels.sort(function (a, b) {
        if (a.price && a.price.options && a.price.options.length > 0 &&
            b.price && b.price.options && b.price.options.length > 0) {

            var priceA = parseFloat(a.price.options[0].formattedDisplayPrice.replace(/[^0-9.]/g, ''));
            var priceB = parseFloat(b.price.options[0].formattedDisplayPrice.replace(/[^0-9.]/g, ''));

            if (sortOrder === 'small-first') {
                return priceA - priceB;
            } else {
                return priceB - priceA;
            }
        }
    });

    sessionStorage.setItem('MakkahHotels', JSON.stringify(sessionHotels));
    AppendMakkahHotels(sessionHotels);
}

function AppendMakkahHotels(HotelsList) {
    TotalHotels = HotelsList.length;
    var a = "" + JSON.stringify(HotelsList) + "";
    var data = { hotels: a, city:"Makkah" }
    $.ajax('/Hotels/RenderHotelsonPartial', {
        method: "POST",
        data: data,
        success: function (data, xhr, status) {
            $("#MakkahHotelsCards").children(".position-relative").remove();
            $("#MakkahHotelsCards").append(data);
        },
        error: function (error) {
            console.log('Error fetching more Hotels:', error);
        }
    });
    $("#TotalMakkahHotels").text(TotalHotels)
}
function addCommas(number) {
    return "$"+number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function removeCommas(number) {
    return parseFloat(number.toString().replace(/,/g, ''));
}
$(document).on("click", "#showmoredropdowns", function () {

    var dropdowncounter = 1;

    var newDropdown = $('<select class="w-100"><option value="0">0</option><option value="1">1</option>' +
        '<option value="2">2</option><option value="3">3</option><option value="4">4</option></select></div>');

    $("#dropdowns").append(newdropdown);

    dropdowncounter++;
});
$(document).on("change", "#dropdown", function () {
    PriceSortingOrder = $(this).val();
    $("dropdownselected").text(PriceSortingOrder);
    if (PriceSortingOrder == "small-first")
        $("#MakkahHotelsSortedBy").text("Sorted By Price (Lowest Price)");
    else
        $("#MakkahHotelsSortedBy").text("Sorted By Price (Highest Price)");
    var sessionHotels = JSON.parse(sessionStorage.getItem('MakkahHotels'));
    sortHotelsByPrice(sessionHotels, PriceSortingOrder);
});
$("#MakkahHotelsSearchbar").on("keyup", function () {
    var query = $("#MakkahHotelsSearchbar").val().toLocaleLowerCase();
    hotels.splice(0, hotels.length)
    var sessionHotels = JSON.parse(sessionStorage.getItem('MakkahHotels'));
    var filteredHotels = sessionHotels.filter(function (hotel) {
        var matchhotel = hotel.name.toLowerCase().includes(query);
        if (matchhotel) {
            hotels.push(hotel);
        }
    });
    AppendMakkahHotels(hotels);
});
$(document).on("click",`[makkah-hotel-id]`, function () {
    var hotelid = $(this).attr("makkah-hotel-id");
    var sessionHotels = JSON.parse(sessionStorage.getItem('MakkahHotels'));
     sessionHotels.filter(function (hotel) {
         if (hotel.id == hotelid) {
             $(`[Card-Image="MakkahHotelDetailsImage"]`).attr("src", `${hotel.imageUrl}`);
             $("#MakkahHotelDetailsName").text(hotel.name);
             $("#MakkahHotelDetailsDistance").text(hotel.destinationInfo.distanceFromDestination.value+"km");
             $("#MakkahHotelDetailsPrice").text(hotel.price.options[0].formattedDisplayPrice);
        }
     });
});

function GetValuesForFlights() {

    var departure = sessionStorage.getItem("outbounddeparture");
    var destination = sessionStorage.getItem("outbounddestination");
    var start = sessionStorage.getItem("outbounddepartdate");
    var inbounddep = sessionStorage.getItem("inbounddeparture");
    var inbounddest = sessionStorage.getItem("inbounddestination");
    var departdate = sessionStorage.getItem("inbounddeparturedate");
    var totalprice = sessionStorage.getItem("totalprice");


    $("#origincode").text(departure);
    $("#destinationcode").text(destination);
    $("#departuredate").text(start);
    $("#flight_total_price").text(totalprice);
    $("#departure_inbound_code").text(inbounddep);
    $("#destination_inbound_code").text(inbounddest);
    $("#arrivaldate").text(departdate);
}