using System.Security.Claims;
using DataAccessLayer.Repositories.Renting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace RentingCars.Controllers;

[Authorize]
public class RentingController : Controller
{
    private readonly IStringLocalizer<RentingController> _localizer;
    private readonly IRentingRepository _rentingRepository;
    public RentingController(IStringLocalizer<RentingController> localizer, IRentingRepository rentingRepository)
    {
        _localizer = localizer;
        _rentingRepository = rentingRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Rent (Guid carId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        bool isRent = await _rentingRepository.RentCar(userId, carId);
        if (isRent) TempData["successMessage"] = _localizer["Car booked successfully"].Value;
        else TempData["ErrorMessage"] = _localizer["Booking failed"].Value;
        
        return RedirectToAction("Index" , "Home");
    }
}