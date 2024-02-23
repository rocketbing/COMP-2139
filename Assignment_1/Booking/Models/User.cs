using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Booking.Models;

public class User : IdentityUser
{
    [MaxLength(128)]
    public string FirstName { get; set; }
    [MaxLength(128)]
    public string LastName { get; set; }
}
