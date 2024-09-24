using System.ComponentModel.DataAnnotations;

namespace UmrahBooking.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; } 
        public string BookingDate { get; set; }
        public string TravelDate { get; set; }
        public string Status { get; set; }
        public string Invoice { get; set; }
    }
}
