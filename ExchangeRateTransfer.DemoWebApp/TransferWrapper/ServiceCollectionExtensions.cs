using ExchangeRateTransfer.DemoWebApp.DataContext;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Services;
using ExchangeRateTransfer.Worker.DependencyInjection;

namespace ExchangeRateTransfer.DemoWebApp.TransferWrapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection EnableExchangeRateTransfer(this IServiceCollection services, Func<Utilities.ITransferSettings, Utilities.ITransferSettings>? settings = null)
        {
            DotNet.DependencyInjection.ServiceCollectionExtensions
                .UseExchangeRateTransferServices<DemoWebAppDbContext, ExchangeRate, long>(
                    services,
                    settings
                );

            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            _ = AddExchangeRateTransferBackgroundService(services);

            return services;
        }


        private static IServiceCollection AddExchangeRateTransferBackgroundService(this IServiceCollection services)
        {
            Worker.DependencyInjection.ServiceCollectionExtensions
                .AddExchangeRateTransferWorker(services)
                .AddExchangeRateTransferBackgroundService<ExchangeRate, long>();

            return services;
        }
    }
}
