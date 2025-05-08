using DataAccessLayer.Repositories.Renting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Jobs;

public class RentalCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RentalCleanupService> _logger;

    public RentalCleanupService(IServiceProvider serviceProvider, ILogger<RentalCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var rentingRepo = scope.ServiceProvider.GetRequiredService<IRentingRepository>();
                var removed = await rentingRepo.RemoveExpiredRentalsAsync();
                _logger.LogInformation($"{removed} expired rentals cleaned up at {DateTime.UtcNow}.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
