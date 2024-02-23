using Booking.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        this._roleManager = roleManager;
    }
    public async Task<IActionResult> Index()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }

    public async Task<IActionResult> NewRole(string Id)
    {
        UserRoleDto model = new UserRoleDto();

        if (Id != null)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            model.RoleName = role.Name;
            model.Id = role.Id;
        }
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> NewRole(UserRoleDto model)
    {
        if (ModelState.IsValid)
        {
            if (model.Id != null)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            else
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult identityResult = await _roleManager.CreateAsync(identityRole);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
        }
        return View(model);
    }
}
