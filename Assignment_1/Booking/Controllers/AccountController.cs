using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booking.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Login(string returlUrl = null)
    {
        ViewData["ReturnUrl"] = returlUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto model)
    {
        // Check if it's a guest login attempt
        if (ModelState.IsValid)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"Username '{model.UserName}' not found.");
                return View(model);
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"Incorrrect Password!!!");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        }
        return View(model);
    }

    public IActionResult Register()
    {
        UserRegistrationDto model = new UserRegistrationDto();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegistrationDto model)
    {
        try
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
                    if (userResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Customer");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("index", "Home");
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
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Username: '{model.UserName}' already exists.");
                    return View(model);
                }
            }
            return View(model);
        }
        catch (Exception ex)
        {

            throw;
        }
        
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
