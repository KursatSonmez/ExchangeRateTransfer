using ExchangeRateTransfer.DotNet.Data.EfContext;
using ExchangeRateTransfer.DotNet.Services.Abstract;
using ExchangeRateTransfer.ExchangeRateReader;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Xml;

namespace ExchangeRateTransfer.DotNet.Services.Concrete
{
    public class LiraExchangeRateTransferService<T, PK> : ILiraExchangeRateTransferService
        where T : Data.Entities.ExchangeRate<PK>, new()
        where PK : struct
    {
        protected readonly ILiraExchangeRateReader _reader;
        protected readonly IExchangeRateTransferDbContext<T, PK> _dbContext;

#pragma warning disable S2743 // Static fields should not be used in generic types
        private static readonly System.Globalization.CultureInfo Culture = new("tr-TR");
#pragma warning restore S2743 // Static fields should not be used in generic types

        public LiraExchangeRateTransferService(
                ILiraExchangeRateReader reader,
                IExchangeRateTransferDbContext<T, PK> dbContext
            )
        {
            _reader = reader;
            _dbContext = dbContext;
        }

        protected DbSet<T> DbSet => _dbContext.Set<T>();

        protected virtual Expression<Func<T, bool>> GetSelectExpression(DateTimeOffset date)
            => x => x.ExchangeRateDate.Date == date.Date;

        public Task<bool> AnyAsync(DateTimeOffset exchangeRateDate, CancellationToken cancellationToken = default)
        {
            return DbSet.AnyAsync(GetSelectExpression(exchangeRateDate), cancellationToken);
        }

        public bool Any(DateTimeOffset exchangeRateDate)
        {
            return DbSet.Any(GetSelectExpression(exchangeRateDate));
        }

        public async Task<int> ReadAndSaveIfNotAsync(DateTimeOffset exchangeRateDate, CancellationToken cancellationToken = default)
        {
            // Eğer bu tarihe ait veri varsa herhangi bir işlem yapılmaz
            if (await AnyAsync(exchangeRateDate, cancellationToken))
                return 0;

            var list = LoadExchangeRate(exchangeRateDate);

            await DbSet.AddRangeAsync(list, cancellationToken: cancellationToken);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public int ReadAndSaveIfNot(DateTimeOffset exchangeRateDate)
        {
            // Eğer bu tarihe ait veri varsa herhangi bir işlem yapılmaz
            if (Any(exchangeRateDate))
                return 0;

            var list = LoadExchangeRate(exchangeRateDate);

            DbSet.AddRange(list);

            return _dbContext.SaveChanges();
        }

        #region Helpers

        public virtual T[] LoadExchangeRate(DateTimeOffset? specificDate)
        {
            var xmlDocument = _reader.Read(specificDate);

            return ConvertXmlToExchangeRate(xmlDocument, specificDate).ToArray();
        }

        /// <summary>
        /// XML Dokümanını okur ve içindeki bilgileri T olarak geri döner
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="exchangeRateDate">
        /// ExchangeRateDate değerinin ne olduğunu belirlemek için kullanılır.
        /// Yani verilerin hangi tarihe ait olduğunu temsil eder.
        /// 
        /// Eğer null ise DateTime.Now değerini alır.
        /// </param>
        protected virtual IEnumerable<T> ConvertXmlToExchangeRate(XmlDocument xmlDocument, DateTimeOffset? exchangeRateDate = null)
        {
            return ConvertXmlToExchangeRateModel(xmlDocument, exchangeRateDate);
        }

        /// <summary>
        /// Converts node content to decimal value according to nullable condition
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nullable">
        /// If TRUE -> returns NULL if node content is empty
        /// FALSE -> return 1 if node content is empty
        /// </param>
        /// <returns></returns>
        protected static decimal? ToDecimal(XmlNode node, bool nullable = false)
        {
            if (nullable && string.IsNullOrWhiteSpace(node.InnerText))
                return null;

            string text;
            if (!nullable && string.IsNullOrWhiteSpace(node.InnerText))
                text = "1";
            else
                text = node.InnerText;
            //throw new ArgumentNullException(nameof(node.InnerText), $"node.InnerText is null! NodeName={node.Name}")

            var ci = (System.Globalization.CultureInfo.InvariantCulture.Clone() as System.Globalization.CultureInfo)!;
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            var a = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			var a2 = System.Globalization.CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;

			return decimal.Parse(text, ci);
        }

        protected static int ToInt(XmlNode node)
            => string.IsNullOrWhiteSpace(node.InnerText)
            ? 0
            : Convert.ToInt32(node.InnerText);

        /// <summary>
        /// XML Dokümanını okur ve içindeki bilgileri T olarak geri döner
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="exchangeRateDate">
        /// ExchangeRateDate değerinin ne olduğunu belirlemek için kullanılır.
        /// Yani verilerin hangi tarihe ait olduğunu temsil eder.
        /// 
        /// Eğer null ise DateTime.Now değerini alır.
        /// </param>
        public static IEnumerable<T> ConvertXmlToExchangeRateModel(XmlDocument xmlDocument, DateTimeOffset? exchangeRateDate = null, Action<XmlNode, T>? onCreation = null)
        {
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Tarih_Date");
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes == null)
                    continue;

                DateTimeOffset tarih_ddMMyyyy = Convert.ToDateTime(node.Attributes.GetNamedItem("Tarih")!.Value, Culture);

                string bulletinNo = node.Attributes.GetNamedItem("Bulten_No")!.Value!;

                foreach (var child in node.Cast<XmlNode>())
                {
                    XmlNode unit = child["Unit"]!;
                    XmlNode isim = child["Isim"]!;
                    XmlNode currencyName = child["CurrencyName"]!;
                    XmlNode forexBuying = child["ForexBuying"]!;
                    XmlNode forexSelling = child["ForexSelling"]!;
                    XmlNode banknoteBuying = child["BanknoteBuying"]!;
                    XmlNode banknoteSelling = child["BanknoteSelling"]!;
                    XmlNode crossRateUSD = child["CrossRateUSD"]!;
                    XmlNode crossRateOther = child["CrossRateOther"]!;

                    var res = new T()
                    {
                        Kod = child.Attributes!.GetNamedItem("Kod")!.Value!.Trim(),
                        CurrencyCode = child.Attributes.GetNamedItem("CurrencyCode")!.Value!.Trim(),

                        ExchangeRateDate = (exchangeRateDate ?? DateTimeOffset.Now).DateTime,
                        ReleaseDate = tarih_ddMMyyyy.DateTime,
                        Unit = ToInt(unit),
                        ForexBuying = ToDecimal(forexBuying)!.Value,
                        ForexSelling = ToDecimal(forexSelling)!.Value,
                        BanknoteBuying = ToDecimal(banknoteBuying)!.Value,
                        BanknoteSelling = ToDecimal(banknoteSelling)!.Value,
                        CurrencyName = currencyName.InnerText.Trim(),
                        BulletinNo = bulletinNo.Trim(),
                        CrossRateOther = ToDecimal(crossRateOther, true),
                        CrossRateUSD = ToDecimal(crossRateUSD, true),
                        Isim = isim.InnerText.Trim(),
                    };

                    res.OnInsert();

                    onCreation?.Invoke(child, res);

                    yield return res;
                }
            }
        }

        #endregion
    }
}
