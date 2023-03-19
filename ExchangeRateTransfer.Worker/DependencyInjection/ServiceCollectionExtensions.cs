using ExchangeRateTransfer.DotNet.Data.EfContext;
using ExchangeRateTransfer.DotNet.Data.Entities;
using ExchangeRateTransfer.DotNet.DependencyInjection;
using ExchangeRateTransfer.Utilities;
using ExchangeRateTransfer.Worker.BackgroundServices;
using ExchangeRateTransfer.Worker.HostedServices;
using ExchangeRateTransfer.Worker.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateTransfer.Worker.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseExchangeRateTransferWorker<TContext, T, ExchangeRatePK>(this IServiceCollection services, Func<ITransferSettings, ITransferSettings>? settings = null)
            where T : ExchangeRate<ExchangeRatePK>, new()
            where TContext : IExchangeRateTransferDbContext<T, ExchangeRatePK>
            where ExchangeRatePK : struct
        {
            services.UseExchangeRateTransferServices<TContext, T, ExchangeRatePK>(settings);

            return services.AddExchangeRateTransferWorker();
        }

        public static IServiceCollection AddExchangeRateTransferWorker(this IServiceCollection services)
        {
            services.AddScoped(typeof(ExchangeRateTransferWorker<,>));

            return services;
        }

        public static IServiceCollection AddExchangeRateTransferHostedService<T, ExchangeRatePK>(this IServiceCollection services)
        where T : ExchangeRate<ExchangeRatePK>
        where ExchangeRatePK : struct
        {
            services.AddHostedService<ExchangeRateTransferHostedService<T, ExchangeRatePK>>();
            return services;
        }

        public static IServiceCollection AddExchangeRateTransferBackgroundService<T, ExchangeRatePK>(this IServiceCollection services)
            where T : ExchangeRate<ExchangeRatePK>
            where ExchangeRatePK : struct
        {
            services.AddHostedService<ExchangeRateTransferBackgroundService<T, ExchangeRatePK>>();
            return services;
        }
    }
}
