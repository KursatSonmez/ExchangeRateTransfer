using System;
using System.Threading;
using System.Xml;
using System.Net;
using ExchangeRateTransfer.Common;

namespace ExchangeRateTransfer.ExchangeRateReader
{
    public abstract class ExchangeRateXmlReader
    {
        /// <summary>
        /// Maximum number of attempts
        /// If the system encountered an error during the request, it will try this number of times.
        /// If the number of attempts is greater than this number, the attempt will not be made.
        /// </summary>
        protected virtual byte WebXmlPageMaxAttempt { get; set; } = 3;

        /// <summary>
        /// URL with daily exchange rate information
        /// </summary>
        protected abstract string TodayUrl { get; }

        /// <summary>
        /// TCMB Format: http://www.tcmb.gov.tr/kurlar/yyyyMM/ddMMyyyy.xml
        /// </summary>
        protected abstract string GetDateUrl(DateTimeOffset date);

        protected abstract System.Globalization.CultureInfo Culture { get; }

        public virtual XmlDocument Read(DateTimeOffset? spesificDate = null)
            => ReadWebXmlPage(spesificDate);

        protected virtual XmlDocument ReadWebXmlPage(DateTimeOffset? specificDate = null, CancellationToken cancellationToken = default)
        {
            string url = GetUrl(specificDate);

            int i = 1;
            while (i <= WebXmlPageMaxAttempt)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    // xml information is returned if read successfully
                    return GetXMLDocumentFromWebPage(url);
                }
                catch (Exception ex)
                {
                    string mess = "Failed to read exchange rate information";
                    if (specificDate.HasValue)
                        mess += $" specificDate = {specificDate.Value.dd_MM_yyyy()}  URL = {url}";
                    else
                        mess += $" (Daily)  URL = {url}";

                    if (i + 1 <= WebXmlPageMaxAttempt)
                        mess += $"   Will try again... (i = {i}/{WebXmlPageMaxAttempt})";

                    Console.WriteLine($"{mess}{Environment.NewLine}{Environment.NewLine}{ex.ToSimpleString()}");

                    cancellationToken.ThrowIfCancellationRequested();

                    // if not read it waits for 10 seconds.
                    Thread.Sleep(10000);
                    ++i;
                }
            }

            throw new InvalidOperationException($"Failed to read exchange rate information! Maximum number of attempts reached ({WebXmlPageMaxAttempt})!!! Url = {url}");
        }

        /// <summary>
        /// It reads the corresponding web page and returns the XML object and information about which page it read.
        /// 
        /// If no <paramref name="date"/> parameter is specified, it returns today's xml information.
        /// 
        /// </summary>
        /// <param name="date">This parameter is used if information for a spesific date will be retrieved rather than today</param>
        protected virtual XmlDocument GetXMLDocumentFromWebPage(string url)
        {
            XmlDocument xmlDoc = new XmlDocument();
            WebRequest request = WebRequest.Create(url);

            using var response = request.GetResponse();
            xmlDoc.Load(response.GetResponseStream());

            return xmlDoc;
        }

        private string GetUrl(DateTimeOffset? specificDate)
            => !specificDate.HasValue || specificDate.Value.Date == DateTimeOffset.Now.Date
                ? TodayUrl
                : GetDateUrl(specificDate.Value);
    }
}
