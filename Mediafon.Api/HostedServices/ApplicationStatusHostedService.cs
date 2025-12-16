using Microsoft.EntityFrameworkCore;
using Mediafon.Api.Data;
using Mediafon.Api.Models;
using Microsoft.AspNetCore.SignalR;
using Mediafon.Api.Hubs;

namespace Mediafon.Api.HostedServices
{
    public class ApplicationStatusHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ApplicationStatusHostedService> _logger;
        private readonly IHubContext<ApplicationsHub> _hubContext;

        public ApplicationStatusHostedService(IServiceScopeFactory scopeFactory, ILogger<ApplicationStatusHostedService> logger,
            IHubContext<ApplicationsHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ApplicationStatusHostedService started.");

            while(!stoppingToken.IsCancellationRequested)
            { 
                await ProcessApplicationAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ProcessApplicationAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MediafonDbContext>();
            
            var completedDelay = DateTime.UtcNow.AddMinutes(-1);

            var newCompletedApplications = await context.ApplicationRequests
                .Where(a => a.Status == "submitted" && a.CreatedAt <= completedDelay)
                .ToListAsync(stoppingToken);

            if (!newCompletedApplications.Any())
            {
                return;
            }

            foreach (var application in newCompletedApplications)
            {
                application.Status = "completed";
            }

            await context.SaveChangesAsync(stoppingToken);

            _logger.LogInformation("Processed {Count} application(s) to completed status.", newCompletedApplications.Count);

            foreach (var application in newCompletedApplications)
            {
                await _hubContext.Clients.Group($"User_{application.UserId}")
                    .SendAsync("ApplicationStatusUpdated", new
                    {
                        ApplicationId = application.Id,
                        Status = application.Status
                    }, stoppingToken);
            }
        }

    }
}
