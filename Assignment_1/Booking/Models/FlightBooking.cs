using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public class FlightBooking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Contact { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual User User { get; set; }
    }
}
