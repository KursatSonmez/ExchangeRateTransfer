using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;
using ExchangeRateTransfer.DotNet.Services.Concrete;
using ExchangeRateTransfer.ExchangeRateReader;
using ExchangeRateTransfer.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExchangeRateTransfer.UnitTests.ServiceTests
{
	public class ExchangeRateServiceTests : TestBase
	{
		[Fact]
		public async Task Test_GetToday()
		{
			string databaseName = GetDatabaseName();

			var dbContext = NewDbContext(databaseName);

			var loaderService = new LiraExchangeRateReader();

			var liraTransferService = new LiraExchangeRateTransferService<ExchangeRate, long>(
					reader: loaderService,
					dbContext: dbContext
				);

			// https://www.tcmb.gov.tr/kurlar/today.xml
			var exchangeRateDate = DateTimeOffset.Now.Date;

			liraTransferService.ReadAndSaveIfNot(exchangeRateDate);

			var exchangeRateService = new ExchangeRateService<ExchangeRate, long>(dbContext);

			// tümü getirilir
			var list = await exchangeRateService.GetTodayOrSpecificDateQuery(exchangeRateDate).ToArrayAsync();

			var usd = list.SingleOrDefault(x => x.CurrencyCode == "USD");
			Assert.NotNull(usd);

			Assert.True(list.All(x => x.ExchangeRateDate.Date == exchangeRateDate));

			
			var correctTodayData = liraTransferService.LoadExchangeRate(null);

			Assert.Equal(correctTodayData.Length, list.Length);
			Assert.True(correctTodayData.All(x => x.ExchangeRateDate.Date == exchangeRateDate));

			var correctUsd = list.Single(x => x.CurrencyCode == "USD");

			Assert.Equal(correctUsd.BanknoteSelling, usd.BanknoteSelling);
		}

		[Fact]
		public async Task Test_GetSpecificDate()
		{
			string databaseName = GetDatabaseName();

			var dbContext = NewDbContext(databaseName);

			var loaderService = new LiraExchangeRateReader();

			var liraTransferService = new LiraExchangeRateTransferService<ExchangeRate, long>(
					reader: loaderService,
					dbContext: dbContext
				);

			// https://www.tcmb.gov.tr/kurlar/202107/13072021.xml
			var exchangeRateDate = new DateTimeOffset(2021, 7, 13, 0, 0, 0, TimeSpan.Zero);

			liraTransferService.ReadAndSaveIfNot(exchangeRateDate);

			var exchangeRateService = new ExchangeRateService<ExchangeRate, long>(dbContext);

			// tümü getirilir
			var list = await exchangeRateService.GetTodayOrSpecificDate(exchangeRateDate);

			Assert.Equal(20, list.Length);

			var usd = list.SingleOrDefault(x => x.CurrencyCode == "USD");
			Assert.NotNull(usd);
			Assert.Equal(8.6318m, usd.BanknoteSelling);

			var eur = list.SingleOrDefault(x => x.CurrencyCode == "EUR");
			Assert.NotNull(eur);
			Assert.Equal(10.2263m, eur.BanknoteSelling);

			// only 3 currencies
			var currencyCodes = new[] { "CHF", "EUR", "USD" };
			list = await exchangeRateService.GetTodayOrSpecificDate(exchangeRateDate, currencyCodes);

			Assert.Equal(3, list.Length);
			Assert.True(
					list.Select(x => x.CurrencyCode).SequenceEqual(currencyCodes)
				);

			var usd2 = list.SingleOrDefault(x => x.CurrencyCode == "USD");
			Assert.NotNull(usd2);
			Assert.Equal(usd.Id, usd2.Id);

			var chf = list.SingleOrDefault(x => x.CurrencyCode == "CHF");
			Assert.NotNull(chf);
			Assert.Equal(9.4460m, chf.BanknoteSelling);
		}
	}
}
