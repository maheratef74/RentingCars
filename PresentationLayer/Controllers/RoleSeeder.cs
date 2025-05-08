using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.User;
using Microsoft.AspNetCore.Identity;

namespace RentingCars.Controllers;

public class RoleSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    public RoleSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }
    public async Task SeedRolesAndAdminAsync()
    {
        string[] roleNames = { "Admin", "Customer" };

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        
        string adminPhone= "01200000000";
        string adminPassword = "123456";

        var existingAdmin = await _userRepository.GetByPhone(adminPhone);
        if (existingAdmin == null)
        {
            var admin = new Admin
            {
                UserName = adminPhone,
                Email = "maheratef600@gmail.com",
                FullName = "Maher Atef",
                PhoneNumber = adminPhone,
                Gender = Gender.man,
                DateOfBirth = DateOnly.Parse("2001-11-18")
            };

            var result = await _userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}