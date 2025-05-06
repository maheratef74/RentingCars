using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser?> _userManager;
    private readonly AppDbContext _dbContext;
    public UserRepository(UserManager<ApplicationUser?> userManager, AppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }
    public async Task<ApplicationUser?> GetById(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser?> GetByPhone(string phone)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(c => c.PhoneNumber == phone);
    }
}