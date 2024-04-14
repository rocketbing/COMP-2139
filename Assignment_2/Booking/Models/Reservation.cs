using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models;

public class Reservation
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int HotelId { get; set; }
    public int CarRentalId { get; set; }
    [ForeignKey("Id")]
    public Flight? Flight { get; set; }
    [ForeignKey("Id")]
    public Hotel? Hotel { get; set; }
    [ForeignKey("Id")]
    public CarRental? CarRental { get; set; }
}
