using Booking.Models;

namespace Booking.Dtos;

public class CustomerBookingViewModal
{
    public CustomerBookingViewModal()
    {
        Hotels = new List<HotelBooking>();
        flights = new List<FlightBooking>();
        Cars = new List<CarBooking>();
    }
    public List<HotelBooking> Hotels { get; set; }
    public List<FlightBooking> flights { get; set; }
    public List<CarBooking> Cars { get; set; }
}
