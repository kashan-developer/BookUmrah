using System.ComponentModel.DataAnnotations;

namespace UmrahBooking.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public string TotalRooms { get; set; }
        public string BookedRooms { get; set; }
        public string RoomPrice { get; set; }
        public string Area { get; set; }
        public string FarFromHaram { get; set; }
        public string Address { get; set; }
        public string Rating { get; set; }
    }

}
