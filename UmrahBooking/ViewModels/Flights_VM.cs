using System.Net.Http.Headers;
using System.Net.Http;
using UmrahBooking.Models;
using UmrahBooking.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static UmrahBooking.ViewModels.Flights_VM;
using UmrahBooking.ViewModels;

namespace UmrahBooking.ViewModels
{
    public class Flights_VM
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public Flights_VM(ILogger<HomeController> logger, DataBaseContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        //for amadous
        //public class FlightDetails : Flight
        //{
        //    public string? sort { get; set; }
        //    public int? max { get; set; }
        //    public string DestinationTime { get; set; }
        //    public bool? oneWay { get; set; }
        //    public string lastTicketingDate { get; set; }
        //    public int numberOfBookableSeats { get; set; }
        //}

        //public class FlightApiResponse
        //{
        //    public bool? Status { get; set; }
        //    public long? Timestamp { get; set; }
        //    public string? SessionId { get; set; }
        //    public Data? Data { get; set; }
        //}

        //public class Data
        //{
        //    public int? Adults { get; set; }
        //    public Context Context { get; set; }
        //    public string? sort { get; set; }
        //    public Itinerary[] Itineraries { get; set; }
        //}

        //public class Context
        //{
        //    public string Status { get; set; }
        //    public int TotalResults { get; set; }
        //}

        //public class Itinerary
        //{
        //    public string Id { get; set; }
        //    public Price Price { get; set; }
        //    public Leg[] Legs { get; set; }
        //    public bool IsSelfTransfer { get; set; }
        //    public bool IsProtectedSelfTransfer { get; set; }
        //    public FarePolicy? FarePolicy { get; set; }
        //    public Eco Eco { get; set; }
        //    public Dictionary<string, object> FareAttributes { get; set; }
        //    public bool IsMashUp { get; set; }
        //    public bool HasFlexibleOptions { get; set; }
        //    public double Score { get; set; }
        //}

        //public class Price
        //{
        //    public double Raw { get; set; }
        //    public string Formatted { get; set; }
        //}

        //public class Leg
        //{
        //    public string Id { get; set; }
        //    public Origin Origin { get; set; }
        //    public Destination Destination { get; set; }
        //    public int DurationInMinutes { get; set; }
        //    public int StopCount { get; set; }
        //    public bool IsSmallestStops { get; set; }
        //    public DateTime Departure { get; set; }
        //    public DateTime Arrival { get; set; }
        //    public int TimeDeltaInDays { get; set; }
        //    public Carriers Carriers { get; set; }
        //    public Segment[] Segments { get; set; }
        //}

        //public class Origin
        //{
        //    public string Id { get; set; }
        //    public string Name { get; set; }
        //    public string DisplayCode { get; set; }
        //    public string City { get; set; }
        //    public string Country { get; set; }
        //    public bool IsHighlighted { get; set; }
        //}

        //public class Destination
        //{
        //    public string Id { get; set; }
        //    public string Name { get; set; }
        //    public string DisplayCode { get; set; }
        //    public string City { get; set; }
        //    public string Country { get; set; }
        //    public bool IsHighlighted { get; set; }
        //}

        //public class Carriers
        //{
        //    public MarketingCarrier[] Marketing { get; set; }
        //    public string OperationType { get; set; }
        //}

        //public class MarketingCarrier
        //{
        //    public int Id { get; set; }
        //    public string LogoUrl { get; set; }
        //    public string Name { get; set; }
        //    public string AlternateId { get; set; }
        //    public int AllianceId { get; set; }
        //    public string DisplayCode { get; set; }
        //}

        //public class Segment
        //{
        //    public string Id { get; set; }
        //    public Origin Origin { get; set; }
        //    public Destination Destination { get; set; }
        //    public DateTime Departure { get; set; }
        //    public DateTime Arrival { get; set; }
        //    public int DurationInMinutes { get; set; }
        //    public string FlightNumber { get; set; }
        //    public MarketingCarrier MarketingCarrier { get; set; }
        //    public OperatingCarrier OperatingCarrier { get; set; }
        //}

        //public class OperatingCarrier
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public string AlternateId { get; set; }
        //    public int AllianceId { get; set; }
        //    public string DisplayCode { get; set; }
        //}

        //public class FarePolicy
        //{
        //    public bool IsChangeAllowed { get; set; }
        //    public bool IsPartiallyChangeable { get; set; }
        //    public bool IsCancellationAllowed { get; set; }
        //    public bool  IsPartiallyRefundable { get; set; }
        //}

        //public class Eco
        //{
        //    public double EcoContenderDelta { get; set; }
        //}


        public class Departure
        {
            public string iataCode { get; set; }
            public DateTime at { get; set; }
        }

        public class Arrival
        {
            public string iataCode { get; set; }
            public string terminal { get; set; }
            public DateTime at { get; set; }
        }

        public class Aircraft
        {
            public string code { get; set; }
        }

        public class Operating
        {
            public string carrierCode { get; set; }
        }

        public class Segment
        {
            public Departure departure { get; set; }
            public Arrival arrival { get; set; }
            public string carrierCode { get; set; }
            public string number { get; set; }
            public Aircraft aircraft { get; set; }
            public Operating operating { get; set; }
            public string duration { get; set; }
            public string id { get; set; }
            public int numberOfStops { get; set; }
            public bool blacklistedInEU { get; set; }
        }

        public class Price
        {
            public string currency { get; set; }
            public string total { get; set; }
            public string bases { get; set; }
            public List<Fee> fees { get; set; }
            public string grandTotal { get; set; }
        }

        public class Fee
        {
            public string amount { get; set; }
            public string type { get; set; }
        }

        public class AmenityProvider
        {
            public string name { get; set; }
        }

        public class IncludedCheckedBags
        {
            public int weight { get; set; }
            public string weightUnit { get; set; }
        }

        public class Amenity
        {
            public string description { get; set; }
            public bool isChargeable { get; set; }
            public string amenityType { get; set; }
            public AmenityProvider amenityProvider { get; set; }
        }

        public class FareDetailsBySegment
        {
            public string segmentId { get; set; }
            public string cabin { get; set; }
            public string fareBasis { get; set; }
            public string brandedFare { get; set; }
            public string brandedFareLabel { get; set; }
            public string @class { get; set; }
            public IncludedCheckedBags includedCheckedBags { get; set; }
            public List<Amenity> amenities { get; set; }
        }

        public class TravelerPricing
        {
            public string travelerId { get; set; }
            public string fareOption { get; set; }
            public string travelerType { get; set; }
            public Price price { get; set; }
            public List<FareDetailsBySegment> fareDetailsBySegment { get; set; }
        }

        public class Itinerary
        {
            public string duration { get; set; }
            public List<Segment> segments { get; set; }
        }

        public class Data
        {
            public int? Adults { get; set; }
            public string type { get; set; }
            public string id { get; set; }
            public string source { get; set; }
            public bool instantTicketingRequired { get; set; }
            public bool nonHomogeneous { get; set; }
            public bool oneWay { get; set; }
            public DateTime lastTicketingDate { get; set; }
            public DateTime lastTicketingDateTime { get; set; }
            public int numberOfBookableSeats { get; set; }
            public List<Itinerary> itineraries { get; set; }
            public Price price { get; set; }
            public object pricingOptions { get; set; }
            public List<string> validatingAirlineCodes { get; set; }
            public List<TravelerPricing> travelerPricings { get; set; }
        }

        public class RootObject
        {
            public List<Data> data { get; set; }
            public string? sort { get; set;}
        }

    }
}

