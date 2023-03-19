using ExchangeRateTransfer.DotNet.Data.Entities;
using ExchangeRateTransfer.Utilities;
using ExchangeRateTransfer.Worker.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateTransfer.Worker.BackgroundServices
{
    public class ExchangeRateTransferBackgroundService<T, PK> : BackgroundService
        where T : ExchangeRate<PK>
        where PK : struct
    {
        private readonly ILogger<ExchangeRateTransferBackgroundService<T, PK>> _logger;
        public ExchangeRateTransferBackgroundService(
            ILogger<ExchangeRateTransferBackgroundService<T, PK>> logger,
            ITransferSettings transferSettings,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            Services = serviceProvider;
            TransferSettings = transferSettings;
        }

        public readonly IServiceProvider Services;
        protected readonly ITransferSettings TransferSettings;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ExchangeRateTransferBackgroundService running at: {time}", DateTimeOffset.Now);

                try
                {
                    DoWork(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }

                await Task.Delay(TransferSettings.TimerPeriod, stoppingToken);
            }
        }

        protected virtual void DoWork(CancellationToken cancellationToken)
        {
            using var scope = Services.CreateScope();

            var worker = scope.ServiceProvider.GetRequiredService<ExchangeRateTransferWorker<T, PK>>();

            worker.Execute(cancellationToken);
        }
    }
}
