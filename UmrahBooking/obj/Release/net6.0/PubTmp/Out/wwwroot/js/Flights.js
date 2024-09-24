var adults = 0;
var date = "";
var origin = "";
var destination = "";
var travelClass = "";
var currency = "";
var child = "";

var sortingOrder = null;

var sessionFlights = JSON.parse(sessionStorage.getItem('Flights'));

$(document).ready(function () {
    getFlightDetailsFromSession();
    getFlights();
    getFlightDetailsFromSession();
});

let currentPage = 1;

var exchangeRates = {
    USD: { rate: 1, symbol: "USD" },
    EUR: { rate: 0.85, symbol: "EUR" },
    GBP: { rate: 0.75, symbol: "GBP" }
};
$('#currency').change(function () {
    var selectedCurrency = $(this).val();

    $('.FlightCard').each(function () {
        var $card = $(this);
        var amountInUSDText = $card.find('.flight_price').text().trim(); 

        amountInUSDText = amountInUSDText.replace(/[^\d.]/g, '');

        var amountInUSD = parseFloat(amountInUSDText);
        if (isNaN(amountInUSD)) {
            console.error('Invalid price:', amountInUSDText);
            return; 
        }

        if (!exchangeRates.hasOwnProperty(selectedCurrency)) {
            console.error('Exchange rate for', selectedCurrency, 'not found');
            return;
        }

        var exchangeRate = exchangeRates[selectedCurrency].rate;
        var convertedAmount = amountInUSD * exchangeRate;

        if (isNaN(convertedAmount)) {
            console.error('Invalid conversion. Amount in USD:', amountInUSD);
            return; 
        }

        $card.find('.flight_price').text(convertedAmount.toFixed(2));
        $card.find('.currency-symbol').text(exchangeRates[selectedCurrency].symbol);
    });
});


function getFlights() {
    if (!sessionFlights || sessionFlights.length == 0 || sessionFlights=="undefined") {
        $.ajax(`/Flights/GetAllFlights?origin=${origin}&destination=${destination}&departure=${date}&adults=${adults}&child=${child}&currency=${currency}&travelClass=${travelClass.toUpperCase()}`, {
            success: function (data, xhr, status) {
                $(`[data-id="FlightsDetails"]`).append(data.html);
                sessionStorage.setItem('Flights', JSON.stringify(data.flights));
                sessionFlights = data.flights;
                appendFlightsDetails(sessionFlights);
            }
        });
    }
    else {
        debugger;
        if (sessionFlights) {
            sessionFlights.data.forEach(flight => flight.Adults = adults);
            sessionFlights.sort = sessionStorage.getItem("FlightsSorting");
            appendFlightsDetails(sessionFlights);
        }
        //else if (sessionFlights.flights.data) {
        //    sessionFlights.flights.data.forEach(flight => flight.Adults = adults);
        //    appendFlightsDetails(sessionFlights.flights.data);
        //}
        //else {
        //    // Handle other cases if needed
        //}
    }
}

function appendFlightsDetails(flightsList) {
    debugger;
    var shortFlights = flightsList;
    var a = "" + JSON.stringify(shortFlights) + "";
    var data = { flights: a };
    $.ajax('/Flights/ReturnFlights', {
        method: "POST",
        data: data,
        success: function (data, xhr, status) {
            $(`[data-id="FlightsDetails"]`).remove(`[data-container="Partial-Flight-Details-Card"]`);
            $(`[data-id="FlightsDetails"]`).html(data.html);
            sessionFlights = data.flights;
        },
        error: function (error) {
            console.log('Error fetching more flights:', error);
        }
    });
}

function getFlightDetailsFromSession() {
    adults = sessionStorage.getItem('adults');
    date = sessionStorage.getItem('DepartureDate');
    origin = sessionStorage.getItem('origin');
    destination = sessionStorage.getItem('destination');
    travelClass = sessionStorage.getItem('TravelClass');
    currency = sessionStorage.getItem('currency');
    child = sessionStorage.getItem('child');
}

$(document).on("change", ".price_dropdown", function () {
    debugger;
    var sortingOrder = $(this).val();
    sessionStorage.setItem('FlightsSorting', sortingOrder);
    sessionFlights = JSON.parse(sessionStorage.getItem('Flights'));
    if (sessionFlights.data) {
        sessionFlights.sort = sessionStorage.getItem('FlightsSorting');
        sortFlightsByPrice(sessionFlights.data, sortingOrder);
    }
    //else if (sessionFlights.flights.data) {
    //    sessionFlights.flights.sort = sessionStorage.getItem('FlightsSorting');
    //    sortFlightsByPrice(sessionFlights.flights.data, sortingOrder);
    //}
    //else {
    //    // Handle other cases if needed
    //}
});

function sortFlightsByPrice(flightsList, sortOrder) {
    flightsList.sort(function (a, b) {
        if (a.price.formatted && b.price.formatted) {
            var priceA = parseFloat(a.price.formatted.replace(/[^0-9.]/g, ''));
            var priceB = parseFloat(b.price.formatted.replace(/[^0-9.]/g, ''));

            if (sortOrder === 'small-first') {
                return priceA - priceB;
            } else {
                return priceB - priceA;
            }
        }
    });

    sessionStorage.setItem('Flights', JSON.stringify({ data: flightsList }));
    appendFlightsDetails(flightsList);
}

$(document).on("click", `[data-id="flight_data"]`, function () {
    debugger;
    var matchedflight = [];
    var flightid = $(this).attr("flight-id");
    var filtered = sessionFlights.data.filter(function (flight) {
        if (flightid == flight.id) {
            matchedflight.push(flight);
        }
    });

    var departure = matchedflight[0].itineraries[0].segments[0].departure.iataCode;
    var destination = matchedflight[0].itineraries[0].segments[0].arrival.iataCode;
    var start = matchedflight[0].itineraries[0].segments[0].departure.at;
    var Price = matchedflight[0].price.total;
    var Adults = matchedflight[0].adults;
    var travel = matchedflight[0].travelerPricings[0].fareOption;
    var end = matchedflight[0].itineraries[0].segments[0].arrival.at;

    var data = { departurePlace: departure, destinationPlace: destination, startTime: start, ticketPrice: Price, adults: Adults, travelClass: travel, endTime: end }

    var totalprice = Price * Adults;
    var inbounddep = matchedflight[0].itineraries[0].segments[1].departure.iataCode;
    var inbounddest = matchedflight[0].itineraries[0].segments[1].arrival.iataCode;
    var departdate = matchedflight[0].itineraries[0].segments[1].departure.at;

    sessionStorage.setItem("outbounddeparture", departure);
    sessionStorage.setItem("outbounddestination", destination);
    sessionStorage.setItem("outbounddepartdate", start);
    sessionStorage.setItem("inbounddeparture", inbounddep);
    sessionStorage.setItem("inbounddestination", inbounddest);
    sessionStorage.setItem("inbounddeparturedate", departdate);
    sessionStorage.setItem("totalprice", totalprice);


    $.ajax('/Flights/AddFlightsDetails/', {
        method: "POST",
        data: data,

        success: function (data,xhr,status) {
            
        }
    })
    
});