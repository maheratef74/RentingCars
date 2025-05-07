using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.Renting;

public interface IRentingRepository
{
    Task<bool> RentCar(string customerId, Guid carId);
    Task<IEnumerable<RentalRecord>> GetRentalsByUser(string customerId);
    Task<int> CountRentals();
    Task<IEnumerable<RentalRecord>> GetPagedRentals(int page, int pageSize);
    Task<int> RemoveExpiredRentalsAsync();
}