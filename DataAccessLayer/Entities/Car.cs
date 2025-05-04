namespace DataAccessLayer.Entities;

public class Car
{
    public Guid Id { get; set; }
    public string Model { get; set; }
    public decimal PricePerDay { get; set; }
    public bool IsRented { get; set; }
}