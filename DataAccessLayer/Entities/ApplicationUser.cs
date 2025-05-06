using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    
    public string? ProfilePhoto { get; set; } 
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
}