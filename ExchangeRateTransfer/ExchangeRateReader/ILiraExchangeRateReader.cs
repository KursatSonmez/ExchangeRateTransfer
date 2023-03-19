using System;
using System.Xml;

namespace ExchangeRateTransfer.ExchangeRateReader
{
    public interface ILiraExchangeRateReader
    {
        XmlDocument Read(DateTimeOffset? spesificDate = null);
    }
}
