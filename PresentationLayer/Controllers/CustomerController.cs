using System.Security.Claims;
using DataAccessLayer.Repositories.Renting;
using DataAccessLayer.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentingCars.Dtos;

namespace RentingCars.Controllers;

[Authorize]
public class CustomerController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRentingRepository _rentingRepository;

    public CustomerController(IUserRepository userRepository, IRentingRepository rentingRepository)
    {
        _userRepository = userRepository;
        _rentingRepository = rentingRepository;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var customerAU = await _userRepository.GetById(userId);
        var customerDto = customerAU.ToCustomerDto();

        var Rents = await _rentingRepository.GetRentalsByUser(userId);
        var rentsDtos = new List<RentDto>();
        foreach (var rent in Rents)
        {
            rentsDtos.Add(rent.ToRentDto());            
        }
        return View(rentsDtos);
    }
}