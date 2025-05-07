using DataAccessLayer.Entities;
using RentingCars.Models.Car;

namespace RentingCars.Dtos;

public class CarDto
{
    public Guid Id { get; set; }
    public string Model { get; set; }
    public decimal PricePerMonth { get; set; } 
    public string PhotoUrl { get; set; }
}
public static class CarARExtensions
{
    public static Car ToCar(this AddCarAR addCarAr)
    {
        return new Car()
        {
            AvailableQuantity = addCarAr.AvailableQuantity,
            Model = addCarAr.Model,
            PricePerMonth = addCarAr.PricePerMonth
        };
    }
    public static CarDto ToCarDto(this Car car)
    {
        return new CarDto()
        {
           PhotoUrl = car.PhotoUrl,
           Id = car.Id,
           Model = car.Model,
           PricePerMonth = car.PricePerMonth
        };
    }
}