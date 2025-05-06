using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.Renting;

public interface IRentingRepository
{
    Task<bool> RentCar(string userId, Guid carId);
    Task<IEnumerable<RentalRecord>> GetRentalsByUser(string userId);
    Task<int> CountRentals();
    Task<IEnumerable<RentalRecord>> GetPagedRentals(int page, int pageSize);
}