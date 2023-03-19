using ExchangeRateTransfer.DotNet.Data.EfContext;
using ExchangeRateTransfer.DotNet.Data.Entities;
using ExchangeRateTransfer.DotNet.Services.Abstract;
using ExchangeRateTransfer.DotNet.Services.Concrete;
using ExchangeRateTransfer.ExchangeRateReader;
using ExchangeRateTransfer.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateTransfer.DotNet.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection UseExchangeRateTransferServices<TContext, T, ExchangeRatePK>(
				this IServiceCollection services,
				Func<ITransferSettings, ITransferSettings>? settings = null
			)
			where T : ExchangeRate<ExchangeRatePK>, new()
			where ExchangeRatePK : struct
			where TContext : IExchangeRateTransferDbContext<T, ExchangeRatePK>
		{
			AddSettings(services, settings);

			_ = AddExchangeRateTransferContext<TContext, T, ExchangeRatePK>(services);

			services.AddScoped<ILiraExchangeRateReader, LiraExchangeRateReader>();

			services.AddScoped<ILiraExchangeRateTransferService, LiraExchangeRateTransferService<T, ExchangeRatePK>>();
			services.AddScoped(typeof(IExchangeRateService<,>), typeof(ExchangeRateService<,>));

			return services;
		}

		private static IServiceCollection AddExchangeRateTransferContext<TContext, T, ExchangeRatePK>(
				this IServiceCollection services
			)
			where T : ExchangeRate<ExchangeRatePK>
			where TContext : IExchangeRateTransferDbContext<T, ExchangeRatePK>
			where ExchangeRatePK : struct
		{

			services.AddScoped(typeof(IExchangeRateTransferDbContext<T, ExchangeRatePK>), x => x.GetRequiredService<TContext>());

			return services;
		}

		private static void AddSettings(IServiceCollection services, Func<ITransferSettings, ITransferSettings>? settings = null)
		{
			var defaultSettings = TransferSettings.LoadDefaultValues();

			services.AddSingleton(x => settings?.Invoke(defaultSettings) ?? defaultSettings);
		}
	}
}
