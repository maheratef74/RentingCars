namespace RentingCars.Models.Car;

public class AddCarAR
{
    public string Model { get; set; }
    public decimal PricePerMonth { get; set; } 
    public int AvailableQuantity { get; set; }
    public IFormFile Photo { get; set; }
}