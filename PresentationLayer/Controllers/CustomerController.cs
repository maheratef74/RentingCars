using System.Security.Claims;
using DataAccessLayer.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using RentingCars.Dtos;

namespace RentingCars.Controllers;

public class CustomerController : Controller
{
    private readonly IUserRepository _userRepository;

    public CustomerController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var customerAU = await _userRepository.GetById(userId);
        var customerDto = customerAU.ToCustomerDto();
        return View(customerDto);
    }
}