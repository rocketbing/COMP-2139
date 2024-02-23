using Booking.Data;
using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers;


public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    public ReservationController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        //var res = await _context.Reservations
        //    .Include(b => b.Flight)
        //    .Include(b => b.Hotel)
        //    .Include(b => b.CarRental)
        //    .ToListAsync();
        return View();
    }

    public async Task<IActionResult> HotelBooking(HotelBookingViewModel model)
    {
        if (model.filter != null)
        {
            var hotelList = await _context.Hotels
                .Where(x => x.PricePerNight >= model.filter.MinPrice && x.PricePerNight <= model.filter.MaxPrice)
                .ToListAsync();

            // Retrieve booked hotels during the specified date range
            var bookedHotelIds = await _context.HotelBookings
                .Where(x => model.filter.FromDate <= x.ToDate && model.filter.ToDate >= x.FromDate)
                .Select(x => x.HotelId)
                .ToListAsync();
            if (bookedHotelIds.Any())
            {
                model.Hotel = hotelList.Where(x => !bookedHotelIds.Contains(x.Id)).ToList();
            }
            else
            {
                model.Hotel = hotelList;
            }
        }
        return View(model);
    }

    public async Task<IActionResult> HotelReservation(int hotelId, DateTime fromDate, DateTime toDate)
    {
        HotelBookingViewModel model = new HotelBookingViewModel();
        model.HotelBooking.HotelId = hotelId;
        model.HotelBooking.FromDate = fromDate;
        model.HotelBooking.ToDate = toDate;
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser != null)
        {
            model.HotelBooking.UserID = currentUser.Id;
        }
        model.HotelDetail = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == hotelId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> HotelReservation(HotelBookingViewModel model)
    {
        _context.Add(model.HotelBooking);
        await _context.SaveChangesAsync();
        return RedirectToAction("HotelBookingMessage", new { fromDate = model.HotelBooking.FromDate, todate = model.HotelBooking.ToDate });
    }

    public IActionResult HotelBookingMessage(DateTime fromDate, DateTime todate)
    {
        ViewBag.fromDate = fromDate;
        ViewBag.todate = todate;
        return View();
    }

    public async Task<IActionResult> FlightBooking(FlightViewModel model)
    {
        if (model.filter != null)
        {
            var flightList = await _context.Flights
                .Where(x => x.DepartureCity == model.filter.Departure && x.ArrivalCity == model.filter.Arrival)
                .ToListAsync();

            // Retrieve booked hotels during the specified date range
            var bookedHotelIds = await _context.FlightBooking
                .Where(x => model.filter.Date == x.Date)
                .Select(x => x.FlightId)
                .ToListAsync();
            if (bookedHotelIds.Any())
            {
                model.Flights = flightList.Where(x => !bookedHotelIds.Contains(x.Id)).ToList();
            }
            else
            {
                model.Flights = flightList;
            }
        }
        return View(model);
    }

    public async Task<IActionResult> FlightReservation(int flightId, DateTime date)
    {
        FlightViewModel model = new FlightViewModel();
        model.flightBooking.FlightId = flightId;
        model.flightBooking.Date = date;
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser != null)
        {
            model.flightBooking.UserID = currentUser.Id;
        }
        model.flightDetail = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == flightId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> FlightReservation(FlightViewModel model)
    {
        _context.Add(model.flightBooking);
        await _context.SaveChangesAsync();
        return RedirectToAction("FlightBookingMessage", new { date = model.flightBooking.Date });
    }

    public IActionResult FlightBookingMessage(DateTime date)
    {
        ViewBag.date = date;
        return View();
    }


    public async Task<IActionResult> CarBooking(CarViewModel model)
    {
        if (model.filter != null)
        {
            var flightList = await _context.CarRentals
                .Where(x => x.Location == model.filter.Location)
                .ToListAsync();

            // Retrieve booked hotels during the specified date range
            var bookedHotelIds = await _context.CarBookings
                .Where(x => model.filter.Date == x.Date)
                .Select(x => x.CarRentalId)
                .ToListAsync();
            if (bookedHotelIds.Any())
            {
                model.CarRentals = flightList.Where(x => !bookedHotelIds.Contains(x.Id)).ToList();
            }
            else
            {
                model.CarRentals = flightList;
            }
        }
        return View(model);
    }

    public async Task<IActionResult> CarReservation(int carId, DateTime date)
    {
        CarViewModel model = new CarViewModel();
        model.CarBooking.CarRentalId = carId;
        model.CarBooking.Date = date;
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser != null)
        {
            model.CarBooking.UserID = currentUser.Id;
        }
        model.CarDetail = await _context.CarRentals
                .FirstOrDefaultAsync(m => m.Id == carId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CarReservation(CarViewModel model)
    {
        _context.Add(model.CarBooking);
        await _context.SaveChangesAsync();
        return RedirectToAction("FlightBookingMessage", new { date = model.CarBooking.Date });
    }

    public IActionResult CarBookingMessage(DateTime date)
    {
        ViewBag.date = date;
        return View();
    }

    public async Task<IActionResult> CustomerBooking()
    {
        CustomerBookingViewModal model = new CustomerBookingViewModal();
        var currentUser = await _userManager.GetUserAsync(User);
        model.Hotels = await _context.HotelBookings.Include(x=>x.Hotel).Where(x => x.UserID.Equals(currentUser.Id)).ToListAsync();
        model.Cars = await _context.CarBookings.Include(x=>x.CarRental).Where(x => x.UserID.Equals(currentUser.Id)).ToListAsync();
        model.flights = await _context.FlightBooking.Include(x=>x.Flight).Where(x => x.UserID.Equals(currentUser.Id)).ToListAsync();
        return View(model);
    }

    public async Task<IActionResult> AllBooking()
    {
        CustomerBookingViewModal model = new CustomerBookingViewModal();
        model.Hotels = await _context.HotelBookings.Include(x => x.Hotel).Include(x => x.User).ToListAsync();
        model.Cars = await _context.CarBookings.Include(x => x.CarRental).Include(x=>x.User).ToListAsync();
        model.flights = await _context.FlightBooking.Include(x => x.Flight).Include(x => x.User).ToListAsync();
        return View(model);
    }
}
