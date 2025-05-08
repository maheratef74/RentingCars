namespace DataAccessLayer.Repositories.Car;
using DataAccessLayer.Entities;
public interface ICarRepository
{
    Task Add(Car car);
    Task Delete(Guid carId);
    Task Reduce(Car car);
    Task<Car?> GetById(Guid carId);
    Task<int> Count();
    Task<IEnumerable<Car>> GetPaged(int page, int pageSize);


}