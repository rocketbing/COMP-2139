using Booking.Data;
using Booking.Dtos;
using Booking.Helper;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;

namespace Booking.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly EmailSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
    public AccountController(IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager, EmailSender emailSender, IConfiguration configuration,
         Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
        ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _hostingEnvironment = hostingEnvironment;
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

    public async Task<IActionResult> Register()
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
                var request = _httpContextAccessor.HttpContext.Request;

                var roles = await _roleManager.Roles.ToListAsync();
                var roleList = roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();

                model.Roles = roleList;

                if (userCheck == null)
                {
                    var user = new User
                    {
                        UserName = model.UserName,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = true,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                    };
                    var userResult = await _userManager.CreateAsync(user, model.Password);
                    if (userResult.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                        var domainUrl = $"{request.Scheme}://{request.Host}";

                        var callbackUrl = $"{domainUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";

                        await _emailSender.SendMailBySendGrid(user.Email, "Confirm your email", $"<p>Please confirm your account by clicking <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click here</a>.</p>");

                        await _userManager.AddToRoleAsync(user, "travelers");
                        return RedirectToAction("ConfirmationEmailSent", "Account");
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

    public IActionResult ForgetPassword()
    {
        ForgetPassordDto model = new ForgetPassordDto();
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgetPassword(ForgetPassordDto model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction("ForgetPasswordEmailConfirmation", new { email = model.Email });
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendMailBySendGrid(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            return RedirectToAction("ForgetPasswordEmailConfirmation", new { email = model.Email });
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    public IActionResult ResetPassword(string userId, string code = null)
    {
        var model = new ResetPasswordDto { Code = code, UserId = userId };
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Find the user by the provided reset token
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            // If no user is found for the given reset token, handle accordingly (e.g., show an error message)
            ModelState.AddModelError(string.Empty, "Invalid reset token.");
            return View(model);
        }

        // Reset the password for the user
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

        if (result.Succeeded)
        {
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        // If resetting the password failed, add errors to the model state
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    public async Task<User> FindUserByResetToken(string code)
    {
        // Find the user associated with the given reset token
        var user = await _context.Users.FirstOrDefaultAsync(u => _userManager.VerifyUserTokenAsync(u, TokenOptions.DefaultProvider, UserManager<User>.ResetPasswordTokenPurpose, code).Result);

        return user;
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    public IActionResult ConfirmEmailSuccess()
    {
        return View();
    }

    public IActionResult ConfirmEmailFailed()
    {
        return View();
    }

    public IActionResult ForgetPasswordEmailConfirmation(string email)
    {
        ViewBag.email = email;
        return View();
    }

    public IActionResult ConfirmationEmailSent()
    {
        return View();
    }

    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return RedirectToAction("ConfirmEmailSuccess", "Account");
        }
        else
        {
            return View("ConfirmEmailFailed", "Account");
        }
    }

    public async Task<IActionResult> Profile()
    {
        UserUpdateDto model = new UserUpdateDto();
        var user = await _userManager.GetUserAsync(User);
        model.UserId = user.Id;
        model.FirstName = user.FirstName;
        model.LastName = user.LastName;
        model.Email = user.Email;
        model.Image = user.Image;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(UserUpdateDto model)
    {
        if (model.ImageFile != null)
        {
           model.Image  = FileHandler.UploadFile(model.ImageFile, Path.Combine(_hostingEnvironment.WebRootPath, "ProfileImage"), "ProfileImage");
        }
        var user = await _userManager.FindByIdAsync(model.UserId);
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.Image = model.Image;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("Profile"); // Or return appropriate success response
        }
        else
        {
            // If update failed, handle errors appropriately
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model); // Or return appropriate error response
        }
    }
}

