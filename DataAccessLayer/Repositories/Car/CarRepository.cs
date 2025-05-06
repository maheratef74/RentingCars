using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Car;
using DataAccessLayer.Entities;
using DataAccessLayer.DbContext;
public class CarRepository : ICarRepository
{
    private readonly AppDbContext _dbContext;

    public CarRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public  async Task Add(Car car)
    {
        await _dbContext.Cars.AddAsync(car);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Car car)
    {
        var existingCar = await _dbContext.Cars.FindAsync(car.Id);
        if (existingCar is not null)
        { 
            _dbContext.Cars.Remove(existingCar);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task Reduce(Car car)
    {
        var existingCar = await _dbContext.Cars.FindAsync(car.Id);
        if (existingCar is not null && existingCar.AvailableQuantity >= 1)
        {
            existingCar.AvailableQuantity--;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Car?> GetById(Guid carId)
    {
        return await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == carId);
    }

    public async Task<int> Count()
    {
        return await _dbContext.Cars.CountAsync();
    }

    public async Task<IEnumerable<Car>> GetPaged(int page, int pageSize)
    {
        return await _dbContext.Cars
            .OrderBy(c => c.Model) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}