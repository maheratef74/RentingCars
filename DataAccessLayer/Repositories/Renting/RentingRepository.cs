using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Car;
using DataAccessLayer.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Renting;

public class RentingRepository : IRentingRepository
{
    private readonly ICarRepository _carRepository;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _dbContext;

    public RentingRepository(ICarRepository carRepository, IUserRepository userRepository, AppDbContext dbContext)
    {
        _carRepository = carRepository;
        _userRepository = userRepository;
        _dbContext = dbContext;
    }

    public async Task<bool> RentCar(string customerId, Guid carId)
    {
        var user = await _userRepository.GetById(customerId);
        var car = await _carRepository.GetById(carId);

        if (user == null || car == null || car.AvailableQuantity < 1)
            return false;
        
        var rental = new RentalRecord
        {
            CarId = car.Id,
            CustomerId = user.Id,
            RentedAt = DateTime.UtcNow
        };
        await _dbContext.RentalRecords.AddAsync(rental);
        await _carRepository.Reduce(carId);
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<RentalRecord>> GetRentalsByUser(string customerId)
    {
        return await _dbContext.RentalRecords
            .Include(r => r.Car)
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<int> CountRentals()
    {
        return await _dbContext.RentalRecords.CountAsync();
    }

    public async Task<IEnumerable<RentalRecord>> GetPagedRentals(int page, int pageSize)
    {
        return await _dbContext.RentalRecords
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .OrderByDescending(r => r.RentedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> RemoveExpiredRentalsAsync()
    {
        var expirationDate = DateTime.UtcNow.AddMonths(-1);
        
        var expiredRentals = await _dbContext.RentalRecords
            .Where(r => r.RentedAt <= expirationDate)
            .ToListAsync();

        if (!expiredRentals.Any()) return 0;

        foreach (var rental in expiredRentals)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == rental.CarId);
            if (car != null)
            {
                car.AvailableQuantity++;
            }
        }

        _dbContext.RentalRecords.RemoveRange(expiredRentals);
        await _dbContext.SaveChangesAsync();

        return expiredRentals.Count;
    }
    
    public async Task<bool> IsRent(Guid carId)
    {
        return await _dbContext.RentalRecords.AnyAsync(r => r.CarId == carId);
    }
}