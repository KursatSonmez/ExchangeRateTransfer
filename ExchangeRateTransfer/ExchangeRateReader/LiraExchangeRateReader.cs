using System;
using System.Globalization;

namespace ExchangeRateTransfer.ExchangeRateReader
{
    public class LiraExchangeRateReader : ExchangeRateXmlReader, ILiraExchangeRateReader
    {
        public LiraExchangeRateReader()
        {
        }

        protected override string TodayUrl => "https://www.tcmb.gov.tr/kurlar/today.xml";

        protected override string GetDateUrl(DateTimeOffset date)
            => string.Format("http://www.tcmb.gov.tr/kurlar/{0}{1}/{2}{1}{0}.xml",
                date.Year.ToString("0000"),
                date.Month.ToString("00"),
                date.Day.ToString("00"));

        protected override CultureInfo Culture => new System.Globalization.CultureInfo("tr-TR");
    }
}
