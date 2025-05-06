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

    public async Task<bool> RentCar(string userId, Guid carId)
    {
        var user = await _userRepository.GetById(userId);
        var car = await _carRepository.GetById(carId);

        if (user == null || car == null || car.AvailableQuantity < 1)
            return false;
        
        car.AvailableQuantity--;
        
        var rental = new RentalRecord
        {
            CarId = car.Id,
            UserId = user.Id,
            RentedAt = DateTime.UtcNow
        };

        await _dbContext.RentalRecords.AddAsync(rental);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<RentalRecord>> GetRentalsByUser(string userId)
    {
        return await _dbContext.RentalRecords
            .Include(r => r.Car)
            .Where(r => r.UserId == userId)
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
}