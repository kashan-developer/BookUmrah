var hotels = [];
var PriceSortingOrder;
var TotalHotels = 0;
$(document).ready(function () {
    GetHotelsList();
});
function GetHotelsList() {
    var sessionHotels = JSON.parse(sessionStorage.getItem('MadinahHotels'));
    if (!sessionHotels || sessionHotels.length == 0) {
        TotalHotels = 0;
        $.ajax('/Hotels/GetAllHotels?query=Madinah&CheckinDate=2024-09-02&CheckOutDate=2024-09-02&adults=1&sort=PRICE_LOW_TO_HIGH&regionId=602705', {
            success: function (data, xhr, status) {
                $("#MadinahHotelsCards").append(data.html);
                hotels = data.hotels;
                TotalHotels = data.hotels.length;
                $("#TotalMadinahHotels").text(TotalHotels);
                sessionStorage.setItem('MadinahHotels', JSON.stringify(hotels));
            }
        });
    }
    else {
        AppendMadinahHotels(sessionHotels);
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

    sessionStorage.setItem('MadinahHotels', JSON.stringify(sessionHotels));
    AppendMadinahHotels(sessionHotels);
}

function AppendMadinahHotels(HotelsList) {
    TotalHotels = HotelsList.length;
    var a = "" + JSON.stringify(HotelsList) + "";
    var data = { hotels: a, city:"Madinah" }
    $.ajax('/Hotels/RenderHotelsonPartial', {
        method: "POST",
        data: data,
        success: function (data, xhr, status) {
            $("#MadinahHotelsCards").children(".position-relative").remove();
            $("#MadinahHotelsCards").append(data);
        },
        error: function (error) {
            console.log('Error fetching more Hotels:', error);
        }
    });
    $("#TotalMadinahHotels").text(TotalHotels)
}
function addCommas(number) {
    return "$" + number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
        $("#MadinahHotelsSortedBy").text("Sorted By Price (Lowest Price)");
    else
        $("#MadinahHotelsSortedBy").text("Sorted By Price (Highest Price)");
    var sessionHotels = JSON.parse(sessionStorage.getItem('MadinahHotels'));
    sortHotelsByPrice(sessionHotels, PriceSortingOrder);
});
$("#MadinahHotelsSearchbar").on("keyup", function () {
    var query = $("#MadinahHotelsSearchbar").val().toLocaleLowerCase();
    hotels.splice(0, hotels.length)
    var sessionHotels = JSON.parse(sessionStorage.getItem('MadinahHotels'));
    var filteredHotels = sessionHotels.filter(function (hotel) {
        var matchhotel = hotel.name.toLowerCase().includes(query);
        if (matchhotel) {
            hotels.push(hotel);
        }
    });
    AppendMadinahHotels(hotels);
});
$(document).on("click", `[madinah-hotel-id]`, function () {
    var hotelid = $(this).attr("madinah-hotel-id");
    var sessionHotels = JSON.parse(sessionStorage.getItem('MadinahHotels'));
    sessionHotels.filter(function (hotel) {
        if (hotel.id == hotelid) {
            $(`[Card-Image="MadinahHotelDetailsImage"]`).attr("src", `${hotel.imageUrl}`);
            $("#MadinahHotelDetailsName").text(hotel.name);
            $("#MadinahHotelDetailsDistance").text(hotel.destinationInfo.distanceFromDestination.value + "km");
            $("#MadinahHotelDetailsPrice").text(hotel.price.options[0].formattedDisplayPrice);
        }
    });



});