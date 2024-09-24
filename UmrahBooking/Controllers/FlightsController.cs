using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using UmrahBooking.Models;
using static UmrahBooking.ViewModels.Flights_VM;
using UmrahBooking.ViewModels;
using Newtonsoft.Json;
using UmrahBooking.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;


namespace UmrahBooking.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string RapidApiKey = "6f97073a8bmsh8f6204b5abe8e19p158662jsn1446481781a6";

        public FlightsController(ILogger<HomeController> logger, DataBaseContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllFlights(string origin, string destination, string departure, int adults, int child, string currency, string travelClass)
        {
            try
            {
                var client = new HttpClient();
                var requestUri = $"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode={origin}&destinationLocationCode={destination}&departureDate={departure}&adults={adults}&currencyCode={currency}&travelClass={travelClass}&max=100&nonStop=false";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri),
                    Headers =
            {
                {
                 "Authorization", "Bearer GljA6gDkDgbSxQQkgQEt0hFfAACF"
                }
            },
                };

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var flightApiResponse = JsonConvert.DeserializeObject<RootObject>(jsonResult);

                    if (flightApiResponse != null && flightApiResponse.data != null && flightApiResponse.data.Any())
                    {
                        foreach (var flightData in flightApiResponse.data)
                        {
                            flightData.Adults = adults;
                        }
                        var html = await HtmlHelperExtensions.RenderViewToStringAsync(this, "/Views/Flights/Partials/_FlightDetailsCard.cshtml", flightApiResponse);

                        return Json(new { Flights = flightApiResponse, html = html });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error parsing data from the API");
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error fetching data from the API");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }


       
        [HttpPost]
        public IActionResult AddFlightsDetails(Flight flight)
        {
            try
            {
                var fligh = new Flight
                {
                    DeparturePlace = flight.DeparturePlace,
                    DestinationPlace = flight.DestinationPlace,
                    StartTime = flight.StartTime,
                    TicketPrice = flight.TicketPrice,
                    Adults = flight.Adults,
                    TravelClass = flight.TravelClass,
                    TotalSeats = "2",
                    BookedSeats = "1",
                    EndTime = flight.EndTime
                };
                _context.Flights.Add(fligh);
                _context.SaveChanges();

                

                return Json(fligh);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        public async Task<IActionResult> ReturnFlights(string Flights)
        {
            try
            {
                var FlightArray = JsonConvert.DeserializeObject<RootObject>(Flights);
                if (FlightArray != null)
                {
                    if (FlightArray.sort == "small-first")
                    {
                        var list = FlightArray.data.ToList();
                        list = list.OrderBy(x => x.price.total).ToList();
                        FlightArray.data = list;
                    }
                    if (FlightArray.sort == "large-first")
                    {
                        var list = FlightArray.data.ToList();
                        list = list.OrderByDescending(x => x.price.total).ToList();
                        FlightArray.data = list;
                    }
                    var html = await HtmlHelperExtensions.RenderViewToStringAsync(this, "/Views/Flights/Partials/_FlightDetailsCard.cshtml", FlightArray);
                    return Json(new { Flights = FlightArray, html = html });
                }
                else
                {
                    return BadRequest($"Error: Flights not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


    }
}
