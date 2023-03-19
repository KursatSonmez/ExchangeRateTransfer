using ExchangeRateTransfer.DotNet.Data.Entities;
using ExchangeRateTransfer.Utilities;
using ExchangeRateTransfer.Worker.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateTransfer.Worker.HostedServices
{
    public class ExchangeRateTransferHostedService<T, PK> : IHostedService, IDisposable
        where T : ExchangeRate<PK>
        where PK : struct
    {
        private readonly ILogger<ExchangeRateTransferHostedService<T, PK>> _logger;
        private readonly ITransferSettings _settings;
        private readonly IServiceProvider _services;

        private Timer? _timer = null;

        public ExchangeRateTransferHostedService(
            ILogger<ExchangeRateTransferHostedService<T, PK>> logger,
            ITransferSettings transferSettings,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _services = serviceProvider;
            _settings = transferSettings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ExchangeRateFactoryHostedService running at: {time}", DateTimeOffset.Now);

            _timer = new Timer(
                (e) =>
                {
                    DoWork(cancellationToken);
                },
                null,
                TimeSpan.Zero,
                _settings.TimerPeriod
                );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed ExchangeRateFactoryHostedService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected virtual void DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped ExchangeRateTransferHostedService is working.");

            using var scope = _services.CreateScope();

            var worker = scope.ServiceProvider.GetRequiredService<ExchangeRateTransferWorker<T, PK>>();

            worker.Execute(cancellationToken);
        }


        #region IDisposable

        private void DisposeManagedResources()
        {
            _timer?.Dispose();
        }
        private void DisposeNativeResources()
        {
            _timer = null;
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                DisposeManagedResources();

            DisposeNativeResources();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ExchangeRateTransferHostedService() => Dispose(false);

        #endregion
    }
}
