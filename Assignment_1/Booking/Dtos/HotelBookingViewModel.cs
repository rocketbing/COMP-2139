using Booking.Models;

namespace Booking.Dtos;

public class HotelBookingViewModel
{
    public HotelBookingViewModel()
    {
        Hotel = new List<Hotel>();
        HotelDetail = new Hotel();
        HotelBooking = new HotelBooking();
    }
    public BookingSearch filter { get; set; }
    public List<Hotel> Hotel { get; set; }

    public Hotel HotelDetail { get; set; }
    public HotelBooking HotelBooking { get; set; }
}


public class BookingSearch
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
}

