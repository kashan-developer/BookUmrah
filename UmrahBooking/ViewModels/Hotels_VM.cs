using Newtonsoft.Json;
using System.Security.Policy;
namespace UmrahBooking.ViewModels
{
    public class Hotels_VM
    {
        public List<ApiMakkahHotels> RemoveNullPricesHotels(List<ApiMakkahHotels> hotels)
        {
            foreach (var hotel in hotels)
            {
                if (hotel.Price.OtherPrices != null &&
                    hotel.Price.OtherPrices.Price1 == null &&
                    hotel.Price.OtherPrices.Price2 == null &&
                    hotel.Price.OtherPrices.Price3 == null &&
                    hotel.Price.OtherPrices.Price4 == null &&
                    hotel.PriceOptions[0].FormattedDisplayPrice == null)
                {
                    hotels.Remove(hotel);
                }
            }
            return hotels;
        }
    }



    //Amadous api classes to fetch hotel id against geo info
    public class GeoCode
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Address
    {
        public string CountryCode { get; set; }
    }

    public class Distance
    {
        public double Value { get; set; }
        public string Unit { get; set; }
    }

    public class Hotel
    {
        public string ChainCode { get; set; }
        public string IataCode { get; set; }
        public int DupeId { get; set; }
        public string Name { get; set; }
        public string HotelId { get; set; }
        public GeoCode GeoCode { get; set; }
        public Address Address { get; set; }
        public Distance Distance { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class ApiResponse
    {
        public List<Hotel> Data { get; set; }
    }

    //Makcorps Hotels List
    public class Geocode
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Reviews
    {
        public double Rating { get; set; }
        public int Count { get; set; }
    }

    public class MakcorpsHotel
    {
        public Geocode Geocode { get; set; }
        public string Telephone { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }
        public Reviews Reviews { get; set; }
        public string? Vendor1 { get; set; }
        public string? Price1 { get; set; }
        public string? Vendor2 { get; set; }
        public string? Price2 { get; set; }
        public string? Vendor3 { get; set; }
        public string? Price3 { get; set; }
        public string? Vendor4 { get; set; }
        public string? Price4 { get; set; }
    }

    public class MakcorpsApiResponse
    {
        public List<MakcorpsHotel> Hotels { get; set; }
        //public int TotalHotelCount { get; set; }
        //public int TotalpageCount { get; set; }
        //public int CurrentPageHotelsCount { get; set; }
        //public int CurrentPageNumber { get; set; }
    }



















    //amadous for price
    public class SimplePrice
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }
    }

    public class SimpleOffer
    {
        [JsonProperty("price")]
        public SimplePrice Price { get; set; }
    }

    public class SimpleHotel
    {
        //[JsonProperty("offers")]
        //public List<SimpleOffer> Offers { get; set; }
    }

    public class SimpleHotelLocationResponse
    {
        [JsonProperty("offers")]
        public SimpleOffer[] Offers { get; set; }
        //[JsonProperty("apiResponse")]
        //public SimpleApiResponse ApiResponse { get; set; }
    }

    //public class SimpleApiResponse
    //{
    //    [JsonProperty("data")]
    //    public List<SimpleHotel> Data { get; set; }
    //}

    public class SimpleRootResponse
    {
        [JsonProperty("data")]
        public SimpleHotelLocationResponse[] Data { get; set; }
    }








    public class PlaceInfo
    {
        public string Name { get; set; }
        public double? Rating { get; set; }
    }
    public class GooglePlacesDataModel
    {
        public List<PlaceInfo> Results { get; set; }
    }

    public class ApiHotelsList
    {
        public List<ApiMakkahHotels> Properties { get; set; }
    }

    public class LatitudeLongitude
    {
        public string _Typename { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class MapMarker
    {
        public string _Typename { get; set; }
        public string Label { get; set; }
        public LatitudeLongitude LatLong { get; set; }

    }


    public class ApiMakkahHotels
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Availability Availability { get; set; }
        public DestinationInfo DestinationInfo { get; set; }
        [JsonProperty("price")]
        public PriceDetails Price { get; set; }
        public PriceOption[] PriceOptions { get; set; }
        public PropertyImage PropertyImage { get; set; }
        public MapMarker MapMarker { get; set; }
        [JsonProperty("star")]
        public string? Star { get; set; }

        public string ImageUrl
        {
            get { return PropertyImage?.Image?.Url; }
        }
    }
    public class PriceDetails
    {
        [JsonProperty("__typename")]
        public string TypeName { get; set; }

        public Money Lead { get; set; }

        [JsonProperty("options")]
        public List<PriceOption> Options { get; set; }

        //[JsonProperty("amadeusPrice")]
        //public PriceOption AmadeusPrice { get; set; }
        public OtherHotelPrices OtherPrices { get; set; }


    }
    public class OtherHotelPrices
    {
        public string? Price1 { get; set; }
        public string? Price2 { get; set; }
        public string? Price3 { get; set; }
        public string? Price4 { get; set; }
        public string? Site1 { get; set; }
        public string? Site2 { get; set; }
        public string? Site3 { get; set; }
        public string? Site4 { get; set; }
    }

    public class Availability
    {
        public bool Available { get; set; }
        public int? MinRoomsLeft { get; set; }
    }
    public class PropertyImage
    {
        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public class Image
    {
        [JsonProperty("__typename")]
        public string TypeName { get; set; }
        [JsonProperty("url")]
        public string Url
        {
            get; set;
        }

    }

    public class DestinationInfo
    {
        public HotelDistance DistanceFromDestination { get; set; }
        // Add other properties as needed
    }

    public class HotelDistance
    {
        public string Unit { get; set; }
        public double Value { get; set; }
    }

    public class Price
    {
        [JsonProperty("__typename")]
        public string TypeName { get; set; }
        [JsonProperty("lead")]
        public Money Lead { get; set; }
        // Add other properties as needed
    }

    public class PriceOption
    {
        public Money StrikeOut { get; set; }
        public LodgingPlainMessage Disclaimer { get; set; }
        public string FormattedDisplayPrice { get; set; }
    }

    public class Money
    {
        public decimal Amount { get; set; }
        public Currency CurrencyInfo { get; set; }
        public string Formatted { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        // Add other properties as needed
    }

    public class LodgingPlainMessage
    {
        public string Value { get; set; }
        // Add other properties as needed
    }
    public class GoogleTextSearchResponse
    {
        public GooglePlace[] results { get; set; }
    }

    public class GooglePlace
    {
        public double? rating { get; set; }
        // Add other properties as needed
    }
}

