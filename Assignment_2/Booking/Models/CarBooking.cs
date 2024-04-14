using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public class CarBooking
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

        [ForeignKey("CarRental")]
        public int CarRentalId { get; set; }

        public virtual CarRental CarRental { get; set; }
        public virtual User User { get; set; }
    }
}
