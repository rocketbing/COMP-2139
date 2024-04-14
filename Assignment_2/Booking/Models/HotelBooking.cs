using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models
{
    public class HotelBooking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Contact { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual User User { get; set; }
    }
}
