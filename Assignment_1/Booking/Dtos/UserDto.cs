using System.ComponentModel.DataAnnotations;

namespace Booking.Dtos;

public class UserRegistrationDto
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public string RoleId { get; set; }
}

public class UserRoleDto
{
    [Required]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; }
    public string Id { get; set; }
}
public class UserLoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
public class GetUsersDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string RoleName { get; set; }

}