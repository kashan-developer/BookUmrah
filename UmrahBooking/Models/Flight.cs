using System.ComponentModel.DataAnnotations;

namespace UmrahBooking.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DeparturePlace { get; set; }
        public string DestinationPlace { get; set; }
        public string TotalSeats { get; set; }
        public string BookedSeats { get; set; }
        public string TicketPrice { get; set; }
        public int Adults { get; set; }
        public string TravelClass { get; set; }
        public string? NonStop { get; set; }
    }
}
