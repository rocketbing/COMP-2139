using Booking.Data;
using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booking.Controllers;


[Authorize(Roles = "Admin")]

public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users.Select(c => new GetUsersDto()
        {
            Id = c.Id,
            Name = c.FirstName + " " + c.LastName,
            UserName = c.UserName,
            RoleName = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray()),
        }).ToList();
        return View(users);
    }


    public IActionResult NewUser()
    {
        UserRegistrationDto model = new UserRegistrationDto();
        ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> NewUser(UserRegistrationDto model)
    {
        if (ModelState.IsValid)
        {
            var userCheck = await _userManager.FindByNameAsync(model.UserName);
            if (userCheck == null)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                var userResult = await _userManager.CreateAsync(user, model.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, model.RoleId);
                if (userResult.Succeeded)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    if (userResult.Errors.Count() > 0)
                    {
                        foreach (var error in userResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    if (roleResult.Errors.Count() > 0)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Username: '{model.UserName}' already exists.");
                ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
                return View(model);
            }
        }
        ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
        return View(model);
    }
}
