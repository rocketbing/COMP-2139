using Booking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {        
    }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<CarRental> CarRentals { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<HotelBooking> HotelBookings { get; set; }
    public DbSet<CarBooking> CarBookings { get; set; }
    public DbSet<FlightBooking> FlightBooking { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
