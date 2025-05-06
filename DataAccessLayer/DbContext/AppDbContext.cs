using System.Diagnostics;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContext;

public class AppDbContext :IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // change names of tables that identity make it 
        modelBuilder.Entity<ApplicationUser>(e => e.ToTable("Users"));
        modelBuilder.Entity<IdentityRole>(e => e.ToTable("Roles"));
        modelBuilder.Entity<IdentityUserRole<string>>(e => e.ToTable("UserRoles"));
        modelBuilder.Entity<IdentityUserClaim<string>>(e => e.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<string>>(e => e.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityRoleClaim<string>>(e => e.ToTable("RoleCliams"));
        modelBuilder.Entity<IdentityUserToken<string>>(e => e.ToTable("UserTokens"));
       

        
        modelBuilder.Entity<ApplicationUser>()
            .Property(d => d.Id)
            .HasColumnName("UserId");      
            
        // TPT Configuration: Customer and Admin will have their own tables
        modelBuilder.Entity<Customer>()
            .ToTable("Customers");  // Derived type has its own table
        
        modelBuilder.Entity<Admin>()
            .ToTable("Admins");  // Derived type has its own table
    }

    public DbSet<Car?> Cars { get; set; }
    public DbSet<RentalRecord> RentalRecords { get; set; }
    public DbSet<Customer> Customers { get; set; }
}