using BusinessLogicLayer.Services.File;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Car;
using DataAccessLayer.Repositories.Renting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RentingCars.Dtos;
using RentingCars.Models.Car;
using RentingCars.Models.HelperClass;

namespace RentingCars.Controllers;

[Authorize(Roles = Roles.Admin)]
public class DashboardController : Controller
{
    private readonly IFileService _fileService;
    private readonly ICarRepository _carRepository;
    private readonly IStringLocalizer<DashboardController> _localizer;
    private readonly IRentingRepository _rentingRepository;
    public DashboardController(IFileService fileService, ICarRepository carRepository, IStringLocalizer<DashboardController> localizer, IRentingRepository rentingRepository)
    {
        _fileService = fileService;
        _carRepository = carRepository;
        _localizer = localizer;
        _rentingRepository = rentingRepository;
    }

    [HttpGet]
    public  IActionResult AddCar()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCar(AddCarAR addCarAr)
    {
        if (!ModelState.IsValid)
        {
            return View(addCarAr);
        }

      var imgUrl =  await _fileService.UploadFile(addCarAr.Photo, "images");
      var car = addCarAr.ToCar();
      car.PhotoUrl = imgUrl;
      await _carRepository.Add(car);
      TempData["successMessage"] = _localizer["Car Added successfully"].Value;
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> RentingRecord(int pageNumber = 1, int pageSize = 12)
    {
        var totalCount = await _rentingRepository.CountRentals();
        
        var cars = await _rentingRepository.GetPagedRentals(pageNumber, pageSize);
    
        var pagedCars = new PaginatedList<RentalRecord>(cars, totalCount, pageNumber, pageSize);
    
        return View(pagedCars);
    }
    
    [HttpGet]
    public async Task<IActionResult> AllCars(int pageNumber = 1, int pageSize = 12)
    {
        var totalCount = await _carRepository.Count();
        
        var cars = await _carRepository.GetPaged(pageNumber, pageSize);
    
        var pagedCars = new PaginatedList<Car>(cars, totalCount, pageNumber, pageSize);
    
        return View(pagedCars);
    }
    
    [HttpPost]

    public async Task<IActionResult> DeleteCar(Guid carId)
    {
        var isRent = await _rentingRepository.IsRent(carId);
        if (isRent)
        {
            TempData["ErrorMessage"] = _localizer["Cannot delete the car because it is currently rented by a customer"].Value;
        }
        else
        {
            await _carRepository.Delete(carId);
            TempData["successMessage"] = _localizer["Car deleted successfully"].Value;
        }
        return RedirectToAction("AllCars" , "Dashboard");
    }
}