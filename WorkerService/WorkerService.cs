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
            _logger.LogInformation("Приложение запущено.");

            fiveMinutesTimer = new Timer(async (e) =>
            {
                try
                {
                    await NotifySubscribers();
                    _logger.LogInformation("NotifySubscribers выполнен.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при выполнении NotifySubscribers");
                }

            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));


            specificTimeTimer = TimerHelper.SpecificTime(18, 40, async e =>
            {
                try
                {
                    await CollectDailyStatistics();
                    _logger.LogInformation("Дневная статистика собрана.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при сборе дневной статистики.");
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Приложение остановлено.");

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
            Random rnd = new Random();

            if (rnd.NextDouble() > 0.1)
                await Task.CompletedTask;
            else
                throw new NotImplementedException();
        }

        public async Task NotifySubscribers()
        {
            Random rnd = new Random();

            if (rnd.NextDouble() > 0.1)
                await Task.CompletedTask;
            else
                throw new NotImplementedException();
        }
    }
}
