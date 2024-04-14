using Booking.Models;

namespace Booking.Dtos;

public class CarViewModel
{
    public CarViewModel()
    {
        CarRentals = new List<CarRental>();
        CarDetail = new CarRental();
        CarBooking = new CarBooking();
    }
    public CarRentalSearch filter { get; set; }

    public List<CarRental> CarRentals { get; set; }
    public CarRental CarDetail { get; set; }
    public CarBooking CarBooking { get; set; }
}
public class CarRentalSearch
{
    public DateTime Date { get; set; }
    public string Location { get; set; }
}