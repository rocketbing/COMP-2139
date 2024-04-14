using System.ComponentModel.DataAnnotations;

namespace Booking.Models;

public class CarRental
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Modal { get; set; }
    public string Brand { get; set; }
    public string Location { get; set; }
    public decimal PricePerDay { get; set; }

}
