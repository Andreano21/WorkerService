using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Filters;
using Serilog.Events;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Здесь размещение файлов логов указано относительно docker контейнера.
            //Путь для сохранения логов на хосте указан в docker-compose.yml
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message}{NewLine}{Exception}")
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                                                           .WriteTo.File(@"/app/log/ws_success.log"))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error
                                                                           || e.Level == LogEventLevel.Fatal)
                                                           .WriteTo.File(@"/app/log/ws_error.log"))
                .CreateLogger();

            Log.Information(AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                Log.Information("Запуск приложения.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Ошибка при запуске приложения.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WorkerService>();
                });
    }
}
