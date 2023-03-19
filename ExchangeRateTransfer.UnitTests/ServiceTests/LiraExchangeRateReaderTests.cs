using ExchangeRateTransfer.ExchangeRateReader;
using ExchangeRateTransfer.UnitTests.TestModels;
using System.Xml;
using Xunit;

namespace ExchangeRateTransfer.UnitTests.ServiceTests
{
    public class LiraExchangeRateReaderTests
    {
        [Fact]
        public void CanReadXmlPage()
        {
            var service = new LiraExchangeRateReader();

            // https://www.tcmb.gov.tr/kurlar/202005/22052020.xml
            var exchangeRateDate = new DateTimeOffset(2020, 5, 22, 0, 0, 0, TimeSpan.Zero);

            var xmlDocument = service.Read(exchangeRateDate);

            XmlNodeList node_tarih_dates = xmlDocument.GetElementsByTagName("Tarih_Date");

            Assert.NotNull(node_tarih_dates);
            Assert.Equal(1, node_tarih_dates.Count);

            var node_tarih_date = node_tarih_dates.Item(0);

            Assert.NotNull(node_tarih_date);

            Assert.NotNull(node_tarih_date.Attributes);

            Assert.Equal("2020/99", node_tarih_date.Attributes.GetNamedItem("Bulten_No")?.Value);
            Assert.Equal("05/22/2020", node_tarih_date.Attributes.GetNamedItem("Date")?.Value);

            var node_currencies = node_tarih_date.ChildNodes;
            Assert.Equal(20, node_currencies.Count);

            var node_currency_first = node_currencies[0];

            Assert.NotNull(node_currency_first);
            Assert.NotNull(node_currency_first.Attributes);

            Assert.Equal("Currency", node_currency_first.Name);

            Assert.Equal("USD", node_currency_first.Attributes.GetNamedItem("CurrencyCode")?.Value);

            Assert.Equal("6.8142", node_currency_first?["BanknoteSelling"]?.InnerText);

            Assert.Equal("XDR", node_currencies[^1]?.Attributes?.GetNamedItem("CurrencyCode")?.Value);

            Assert.Empty(node_currencies[^1]?["BanknoteSelling"]?.InnerText ?? "");

            Assert.Equal("1.36226", node_currencies[^1]?["CrossRateOther"]?.InnerText);

        }

        [Theory]
        [MemberData(nameof(CanReadExchangeRatesData))]
        public void CanReadExchangeRates(ExchangeRateTestModel testModel, int expectedCount)
        {
            var service = new LiraExchangeRateReader();

            // https://www.tcmb.gov.tr/kurlar/202109/24092021.xml

            var xmlDocument = service.Read(testModel.ExchangeRateDate);

            var list = DotNet.Services.Concrete.LiraExchangeRateTransferService<ExchangeRateTestModel, Guid>.ConvertXmlToExchangeRateModel(xmlDocument, exchangeRateDate: testModel.ExchangeRateDate)
                .ToArray();

            Assert.Equal(expectedCount, list.Length);

            Assert.True(list.All(x => x.BulletinNo == testModel.BulletinNo));
            Assert.True(list.All(x => x.ExchangeRateDate.Date == testModel.ExchangeRateDate.Date));
            Assert.True(list.All(x => x.ReleaseDate.Date == testModel.ReleaseDate.Date));

            Assert.Contains(list, x => x.CurrencyCode == "USD");
            Assert.Contains(list, x => x.CurrencyCode == "EUR");

            var a = list.Single(x => x.CurrencyCode == testModel.CurrencyCode);

            Assert.Equal(a, testModel);
        }

        public static IEnumerable<object[]> CanReadExchangeRatesData()
        {
            var p1 = new ExchangeRateTestModel()
            {
                ExchangeRateDate = new DateTime(2021, 9, 24, 0, 0, 0),
                ReleaseDate = new DateTime(2021, 09, 24, 0, 0, 0),
                BulletinNo = "2021/178",
                Unit = 1,
                CurrencyCode = "USD",
                CurrencyName = "US DOLLAR",
                BanknoteBuying = 8.8178m,
                BanknoteSelling = 8.8531m,
                ForexBuying = 8.8240m,
                ForexSelling = 8.8399m,
                Kod = "USD",
                Isim = "ABD DOLARI",
                CrossRateUSD = null,
                CrossRateOther = null,
            };
            var p2 = new ExchangeRateTestModel()
            {
                ExchangeRateDate = new DateTime(2021, 8, 24, 0, 0, 0),
                ReleaseDate = new DateTime(2021, 8, 24, 0, 0, 0),
                BulletinNo = "2021/156",
                Unit = 1,
                CurrencyCode = "EUR",
                CurrencyName = "EURO",
                BanknoteBuying = 9.8659m,
                BanknoteSelling = 9.9054m,
                ForexBuying = 9.8728m,
                ForexSelling = 9.8906m,
                Kod = "EUR",
                Isim = "EURO",
                CrossRateUSD = null,
                CrossRateOther = 1.1735m,
            };
            return new List<object[]>()
            {
                new object[]{ p1, 21 },
                new object[]{ p2, 21 },
            };
        }
    }
}
