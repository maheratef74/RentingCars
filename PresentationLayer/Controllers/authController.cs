using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RentingCars.Models.Auth.ActionRequest;

namespace RentingCars.Controllers;

public class authController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IStringLocalizer<authController> _localizer;
    private readonly IUserRepository _userRepository;
    public authController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStringLocalizer<authController> localizer, IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _localizer = localizer;
        _userRepository = userRepository;
    }

    [HttpGet]
    public  IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public  async Task<IActionResult> Register(RegisterAR newCustomer)
    {
        var customerUser = newCustomer.ToCustomer();
        
        var result = await _userManager.CreateAsync(customerUser, newCustomer.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(customerUser, Roles.Customer);
            
            await _signInManager.SignInAsync(customerUser, newCustomer.RememberMe);

            TempData["successMessage"] = _localizer["Account Created successfully"].Value;
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(newCustomer);
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginAR loginAr)
    {
        if (ModelState.IsValid)
        {
            var user = await _userRepository.GetByPhone(loginAr.Phone);
            if (user is not null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginAr.Password);

                if (isPasswordValid)
                {
                    // Create a Cookie
                    await _signInManager.SignInAsync(user, loginAr.RememberMe);
                    if (User.IsInRole(Roles.Customer))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("DailyAppointment", "DashBoard");
                }
            }
            ModelState.AddModelError("Password", _localizer["Username or Password invalid"]);
        } 
        return View(loginAr);
    }
    
    public async Task<IActionResult> CheckPhone(string PhoneNumber, string? PatientId = null)
    {
        var user = await _userRepository.GetByPhone(PhoneNumber);
        if (user is null) return Json(true);
    
        return Json(false);
    }
}