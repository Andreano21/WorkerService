using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class WorkerService : IHostedService, IDisposable, IWorkerService
    {
        private readonly ILogger<WorkerService> _logger;

        private Timer fiveMinutesTimer;
        private Timer specificTimeTimer;

        public WorkerService(ILogger<WorkerService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WorkerService running.");

            fiveMinutesTimer = new Timer(async (e) =>
            {
                try
                {
                    await NotifySubscribers();
                    _logger.LogInformation("Successful task completion: NotifySubscribers()");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while executing task: NotifySubscribers()");
                }

            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(5));


            specificTimeTimer = TimerHelper.SpecificTime(23, 50, async e =>
            {
                try
                {
                    await CollectDailyStatistics();
                    _logger.LogInformation("Successful task completion: CollectDailyStatistics()");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while executing task: CollectDailyStatistics()");
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkerService stoped.");

            fiveMinutesTimer?.Change(Timeout.Infinite, 0);
            specificTimeTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            fiveMinutesTimer?.Dispose();
            specificTimeTimer?.Dispose();
        }

        public async Task CollectDailyStatistics()
        {
            throw new NotImplementedException();
        }

        public async Task NotifySubscribers()
        {
            throw new NotImplementedException();
        }
    }
}
