using System.ComponentModel.DataAnnotations;

namespace Booking.Models;

public class Flight
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Name")]
    public string FlightName { get; set; }
    [Required]
    public string Airline { get; set; }
    [Required]
    [Display(Name = "Departure City")]
    public string DepartureCity { get; set; }
    [Required]
    [Display(Name = "Arrival City")]
    public string ArrivalCity { get; set; }
    [Required]
    [Display(Name = "Departure Date/Time")]
    public DateTime DepartureTime { get; set; }
    [Required]
    [Display(Name = "Arrival Date/Time")]
    public DateTime ArrivalTime { get; set; }
    public decimal Price { get; set; }
}
