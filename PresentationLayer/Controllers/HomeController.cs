using DataAccessLayer.Repositories.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RentingCars.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ICarRepository _carRepository;

    public HomeController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cars = await _carRepository.GetPaged(1, 10);
        return View(cars);
    }
}