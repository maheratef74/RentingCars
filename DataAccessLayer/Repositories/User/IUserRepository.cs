using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.User;

public interface IUserRepository
{
    Task<Customer?> GetById(string userId);
    Task<ApplicationUser?> GetByPhone(string phone);
}