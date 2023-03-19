namespace ExchangeRateTransfer.DotNet.Dtos
{
    public class ExchangeRateSummary<PK> where PK : struct
    {
        public PK Id { get; set; }

        public string? CurrencyCode { get; set; }

        public decimal BanknoteSelling { get; set; }
    }
}
