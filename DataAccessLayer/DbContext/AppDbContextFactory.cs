using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.DbContext;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer("Server=db18769.public.databaseasp.net; Database=db18769; User Id=db18769; Password=r=6N9M!kA-m5; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");
        return new AppDbContext(optionsBuilder.Options);
    }
}