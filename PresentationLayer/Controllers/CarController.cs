using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentingCars.Models.HelperClass;

namespace RentingCars.Controllers;

[Authorize]
public class CarController : Controller
{
    private readonly ICarRepository _carRepository;

    public CarController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet] public async Task<IActionResult> All(int pageNumber = 1, int pageSize = 12)
    {
        var totalCount = await _carRepository.Count();
        
        var cars = await _carRepository.GetPaged(pageNumber, pageSize);
    
        var pagedCars = new PaginatedList<Car>(cars, totalCount, pageNumber, pageSize);
    
        return View(pagedCars);
    }
}