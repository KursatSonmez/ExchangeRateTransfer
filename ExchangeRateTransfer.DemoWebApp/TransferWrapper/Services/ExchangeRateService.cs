using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Dtos;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DemoWebApp.TransferWrapper.Services
{
	public interface IExchangeRateService
	{
		Task<ExchangeRateComboBoxDto[]> GetTodayOrSpecificDate(DateTimeOffset? specificDate = null, bool allCurrencies = true, CancellationToken cancellationToken = default);

		Task<ExchangeRate[]> GetExchangeRatesByDate(DateTimeOffset? specificDate = null, bool allCurrencies = true, CancellationToken cancellationToken = default);
	}

	public class ExchangeRateService : IExchangeRateService
	{
		private readonly DotNet.Services.Abstract.IExchangeRateService<ExchangeRate, long> _exchangeRateService;

		public ExchangeRateService(DotNet.Services.Abstract.IExchangeRateService<ExchangeRate, long> exchangeRateService)
		{
			_exchangeRateService = exchangeRateService;
		}

		public async Task<ExchangeRateComboBoxDto[]> GetTodayOrSpecificDate(DateTimeOffset? specificDate = null, bool allCurrencies = true, CancellationToken cancellationToken = default)
		{
			var currencyCodes = GetCurrencies(allCurrencies);

			return await _exchangeRateService.GetTodayOrSpecificDateQuery(specificDate, currencyCodes)
				.Select(x => new ExchangeRateComboBoxDto
				{
					BanknoteSelling = x.BanknoteSelling,
					CurrencyCode = x.CurrencyCode,
					Id = x.Id,
				})
				.ToArrayAsync(cancellationToken);
		}

		public async Task<ExchangeRate[]> GetExchangeRatesByDate(DateTimeOffset? specificDate = null, bool allCurrencies = true, CancellationToken cancellationToken = default)
		{
			var currencyCodes = GetCurrencies(allCurrencies);

			return await _exchangeRateService.GetTodayOrSpecificDateQuery(specificDate, currencyCodes)
				.ToArrayAsync(cancellationToken);
		}

		private static string[]? GetCurrencies(bool allCurrencies)
			=> allCurrencies ? null : new[] { "USD", "EUR", "CHF" };

	}
}
