using Microsoft.AspNetCore.Mvc.Rendering;
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
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public string RoleId { get; set; }
    public List<SelectListItem> Roles { get; set; }
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

public class ForgetPassordDto
{
    public string Email { get; set; }
}

public class ResetPasswordDto
{
    public string Code { get; set; } // This property will hold the token for password reset verification
    public string UserId { get; set; } // This property will hold the token for password reset verification

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}


public class UserUpdateDto
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public IFormFile ImageFile { get; set; }
}