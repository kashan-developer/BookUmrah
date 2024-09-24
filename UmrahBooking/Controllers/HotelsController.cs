using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;
using UmrahBooking.Models;
using UmrahBooking.Services;
using UmrahBooking.ViewModels;
using static UmrahBooking.ViewModels.Flights_VM;

namespace UmrahBooking.Controllers
{
    public class HotelsController : Controller
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly string BookingComapiKey = "YOUR_BOOKING_API_KEY";
        private readonly HttpClient httpClient;
        private readonly string RapidApiKey = "11dc52453bmshf4d28312cb0c9a3p150cc1jsn071d2076d554";
        private readonly IHttpClientFactory _httpClientFactory;
        public HotelsController(IHttpClientFactory httpClientFactory, IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult MadinahHotel()
        {
            return View();
        }
        public IActionResult MakkahHotel()
        { 
            return View();
        }
        public IActionResult N(string Name)
        {
            HttpContext.Session.SetString("UserName",Name);
            return Json(true);
        }
        public IActionResult M()
        {
            return Json(HttpContext.Session.GetString("UserName"));
        }
        
        public async Task<IActionResult> GetAllHotels(string query, string CheckinDate, string CheckOutDate, int adults, string sort, string regionId)
        {
            try
            {
                string url = "https://hotels-com-provider.p.rapidapi.com/v2/hotels/search";


                var parameters = new
                {
                    query,
                    locale = "en_GB",
                    checkin_date = CheckinDate,
                    checkout_date = CheckOutDate,
                    adults_number = adults,
                    hotel_id = 4039860,
                    domain = "AE",
                    sort_order = sort,
                    region_id = regionId,
                };

                List< ApiMakkahHotels> HotelsList = new List<ApiMakkahHotels>();
                Hotels_VM vm = new Hotels_VM();
                HttpClient client = new HttpClient();
                string queryString = string.Join("&", parameters.GetType().GetProperties()
                    .Select(property => $"{property.Name}={Uri.EscapeDataString(property.GetValue(parameters)?.ToString())}"));
                string fullUrl = $"{url}?{queryString}";
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "hotels-com-provider.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", RapidApiKey);
                HttpResponseMessage response = await client.GetAsync(fullUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ApiHotelsList apiResponse = JsonConvert.DeserializeObject<ApiHotelsList>(responseBody);
                    var hotels = apiResponse.Properties;

                    var makcorpsHotelsUrl = "https://api.makcorps.com/city?cityid=2628879&pagination=1&cur=USD&rooms=1&adults=2&checkin=2024-01-27&checkout=2024-01-29&api_key=65b3cb20e731c164ea217012";
                    HttpResponseMessage makcorpsHotelsResponse = await client.GetAsync(makcorpsHotelsUrl);
                    string makcorpsStringResponse = await makcorpsHotelsResponse.Content.ReadAsStringAsync();

                    int index = makcorpsStringResponse.IndexOf(",[{\"totalHotelCount\":");
                    makcorpsStringResponse = makcorpsStringResponse.Substring(0, index);
                    makcorpsStringResponse = "{" + "\"" + "hotels" + "\":" + makcorpsStringResponse + "]}";
                    Console.WriteLine(makcorpsStringResponse);
                    MakcorpsApiResponse MakcorpsHotels = JsonConvert.DeserializeObject<MakcorpsApiResponse>(makcorpsStringResponse.ToString());
                    foreach (var hotel in MakcorpsHotels.Hotels)
                    {
                        ApiMakkahHotels matchedhotel = hotels.Where(x => x.Name == hotel.Name).FirstOrDefault();
                        if (matchedhotel != null)
                        {
                            matchedhotel.Price.OtherPrices = new OtherHotelPrices();
                            matchedhotel.Price.OtherPrices.Price1 = hotel.Price1;
                            matchedhotel.Price.OtherPrices.Price2 = hotel.Price2;
                            matchedhotel.Price.OtherPrices.Price3 = hotel.Price3;
                            matchedhotel.Price.OtherPrices.Price4 = hotel.Price4;
                            matchedhotel.Price.OtherPrices.Site1 = hotel.Vendor1;
                            matchedhotel.Price.OtherPrices.Site2 = hotel.Vendor2;
                            matchedhotel.Price.OtherPrices.Site3 = hotel.Vendor3;
                            matchedhotel.Price.OtherPrices.Site4 = hotel.Vendor4;
                            HotelsList.Add(matchedhotel);
                        }
                    }
                    List<ApiMakkahHotels> PricedHotels = hotels.Where(x => x.Price.Options.Count()!=0).ToList(); 
                    HotelsList.AddRange(PricedHotels);
                    var html = "";
                    if (query == "Makkah")
                        html = await HtmlHelperExtensions.RenderViewToStringAsync(this, "/Views/Hotels/Partials/_MakkahHotelCard.cshtml", HotelsList);
                    else
                        html = await HtmlHelperExtensions.RenderViewToStringAsync(this, "/Views/Hotels/Partials/_MadinahHotelCard.cshtml", HotelsList);

                    return Json(new { hotels = HotelsList, html = html });
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error Response Body: " + responseBody);
                    TempData["ErrorMessage"] = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Json($"Error: {ex.ToString()}");
            }

        }

        public async Task<string> GetHotelPrices(string hotelName)
        {
            httpClient.BaseAddress = new Uri("https://partner.api.booking.com/");
            string apiUrl = $"search/v3/hotels?search-term={hotelName}&client_id={BookingComapiKey}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
        public async Task<ActionResult> GetHotelDataFromGooglePlaces()
        {
            try
            {
                var apiKey = "AIzaSyB41DRUbKWJHPxaFjMAwdrzWzbVKartNGg";


                var apiUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=21.42280932702965,39.825797058309504&radius=3000&type=lodging&key={apiKey}";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    PlaceInfo apiResponse = JsonConvert.DeserializeObject<PlaceInfo>(data);
                    List<PlaceInfo> result = new List<PlaceInfo>();

                    //var places = data.Results.Select(result => new PlaceInfo
                    //{
                    //    Name = result.Name,
                    //    Rating = result.Rating ?? 0 // Assume 0 if rating is not available
                    //}).ToList();

                    return Json(result);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> RenderHotelsonPartial(string Hotels, string City)
        {
            try
            {
                var HotelsArray = JsonConvert.DeserializeObject<List<ApiMakkahHotels>>(Hotels);
                if (HotelsArray != null)
                {
                    var html = "";
                    if (City == "Makkah")
                        html = await HtmlHelperExtensions.RenderViewToStringAsync(this, "/Views/Hotels/Partials/_MakkahHotelCard.cshtml", HotelsArray);
                    if (City == "Madinah")
                        html = await HtmlHelperExtensions.RenderViewToStringAsync(this, "/Views/Hotels/Partials/_MadinahHotelCard.cshtml", HotelsArray);
                    return Json(html);
                }
                else
                {
                    return BadRequest($"Error: Hotels not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetRatings(string placeId)
        {
            try
            {
                var ratings = await GetHotelRatingByName(placeId);
                return Json(new { Rating = ratings });
            }
            catch (Exception ex)
            {
                return Json(new { Error = "Failed to fetch hotel ratings" });
            }
        }
        public async Task<double?> GetHotelRatingByName(string hotelName)
        {
            try
            {
                var apiKey = "AIzaSyB41DRUbKWJHPxaFjMAwdrzWzbVKartNGg";
                //var apiUrl = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={hotelName}&key={apiKey}";
                var apiUrl = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Uri.EscapeDataString(hotelName)}&key={apiKey}";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var details = await response.Content.ReadAsStringAsync();
                    GoogleTextSearchResponse apiResponse = JsonConvert.DeserializeObject<GoogleTextSearchResponse>(details);
                    var firstresult = apiResponse?.results?.FirstOrDefault();
                    return firstresult?.rating;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
