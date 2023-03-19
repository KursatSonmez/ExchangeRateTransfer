namespace ExchangeRateTransfer.DotNet.Services.Abstract
{
    public interface IExchangeRateService<T, PK>
        where T : Data.Entities.ExchangeRate<PK>
        where PK : struct
    {
        IQueryable<T> GetTodayOrSpecificDateQuery(DateTimeOffset? specificDate = null, string[]? currencyCodes = null);

        Task<Dtos.ExchangeRateSummary<PK>[]> GetTodayOrSpecificDate(DateTimeOffset? specificDate = null, string[]? currencyCodes = null, CancellationToken cancellationToken = default);
    }
}
