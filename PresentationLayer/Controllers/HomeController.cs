using Microsoft.AspNetCore.Mvc;

namespace RentingCars.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public  IActionResult Index()
    {
        return View();
    }
}