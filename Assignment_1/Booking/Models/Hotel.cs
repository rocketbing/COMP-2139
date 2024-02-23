using System.ComponentModel.DataAnnotations;

namespace Booking.Models;

public class Hotel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Location { get; set; }
    public int? Rating { get; set; }
    public string Amenities { get; set; }

    [Required]
    [Display(Name = "No Of Rooms")]
    public int NumberOfRooms { get; set; }

    [Display(Name = "Price Per Night")]
    public decimal PricePerNight { get; set; }

}
