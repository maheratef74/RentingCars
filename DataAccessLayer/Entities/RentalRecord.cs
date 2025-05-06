namespace DataAccessLayer.Entities;

public class RentalRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CarId { get; set; }
    public Car Car { get; set; }
    
    public string UserId { get; set; }
    public Customer Customer { get; set; }
    public DateTime RentedAt { get; set; } = DateTime.UtcNow;
}