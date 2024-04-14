using Booking.Models;

namespace Booking.Dtos;

public class FlightViewModel
{
    public FlightViewModel()
    {
        Flights = new List<Flight>();
        flightDetail = new Flight();
        flightBooking = new FlightBooking();
    }
    public FlightSearch filter { get; set; }
    public List<Flight> Flights { get; set; }

    public Flight flightDetail { get; set; }
    public FlightBooking flightBooking { get; set; }
}
public class FlightSearch
{
    public DateTime Date { get; set; }
    public string Departure { get; set; }
    public string Arrival { get; set; }
}

