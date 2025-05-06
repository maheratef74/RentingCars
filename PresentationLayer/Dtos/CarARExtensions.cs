using DataAccessLayer.Entities;
using RentingCars.Models.Car;

namespace RentingCars.Dtos;

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
}