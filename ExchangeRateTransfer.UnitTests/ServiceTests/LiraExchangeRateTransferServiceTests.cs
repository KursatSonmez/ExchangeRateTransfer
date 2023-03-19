using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;
using ExchangeRateTransfer.ExchangeRateReader;
using ExchangeRateTransfer.UnitTests.Base;
using Xunit;

namespace ExchangeRateTransfer.UnitTests.ServiceTests
{
    public class LiraExchangeRateTransferServiceTests : TestBase
    {
        [Fact]
        public async Task CanReadAndSave()
        {
            string databaseName = GetDatabaseName();

            var dbContext = NewDbContext(databaseName);

            var loaderService = new LiraExchangeRateReader();

            var service = new DotNet.Services.Concrete.LiraExchangeRateTransferService<ExchangeRate, long>(
                    reader: loaderService,
                    dbContext: dbContext
                );

            // https://www.tcmb.gov.tr/kurlar/202109/24092021.xml
            var exchangeRateDate = new DateTimeOffset(2021, 9, 24, 0, 0, 0, TimeSpan.Zero);
            var count = service.ReadAndSaveIfNot(exchangeRateDate);

            Assert.Equal(21, count);

            Assert.True(
                await service.AnyAsync(exchangeRateDate)
                );

            var usd = dbContext.ExchangeRates.Single(x => x.CurrencyCode == "USD" && x.ExchangeRateDate.Date == exchangeRateDate.Date);
            Assert.Equal(8.8531m, usd.BanknoteSelling);

            Assert.Equal(
                    21,
                    dbContext.ExchangeRates.Count()
                );

            await dbContext.DisposeAsync();
        }
    }
}
