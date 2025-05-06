namespace DataAccessLayer.Entities;

public class Car
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Model { get; set; }
    public decimal PricePerMonth { get; set; } 
    public int AvailableQuantity { get; set; }
    public string PhotoUrl { get; set; }
}