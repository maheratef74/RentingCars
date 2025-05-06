using BusinessLogicLayer.Services.File;
using DataAccessLayer.Repositories.Car;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RentingCars.Dtos;
using RentingCars.Models.Car;

namespace RentingCars.Controllers;

public class DashboardController : Controller
{
    private readonly IFileService _fileService;
    private readonly ICarRepository _carRepository;
    private readonly IStringLocalizer<DashboardController> _localizer;
    public DashboardController(IFileService fileService, ICarRepository carRepository, IStringLocalizer<DashboardController> localizer)
    {
        _fileService = fileService;
        _carRepository = carRepository;
        _localizer = localizer;
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
}