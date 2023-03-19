using ExchangeRateTransfer.Common;
using ExchangeRateTransfer.DotNet.Data.EfContext;
using ExchangeRateTransfer.DotNet.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExchangeRateTransfer.DotNet.Services.Concrete
{
    public class ExchangeRateService<T, PK> : IExchangeRateService<T, PK>
        where T : Data.Entities.ExchangeRate<PK>
        where PK : struct
    {
        protected readonly IExchangeRateTransferDbContext<T, PK> _dbContext;

        public ExchangeRateService(
                IExchangeRateTransferDbContext<T, PK> dbContext
            )
        {
            _dbContext = dbContext;
        }

        protected DbSet<T> DbSet => _dbContext.Set<T>();

        protected virtual Expression<Func<T, bool>> GetSelectExpression(DateTimeOffset date)
            => x => x.ExchangeRateDate.Date == date.Date;

        public virtual IQueryable<T> GetTodayOrSpecificDateQuery(DateTimeOffset? specificDate = null, string[]? currencyCodes = null)
        {
            var date = specificDate ?? DateTimeOffset.Now;

            Expression<Func<T, bool>> exp = GetSelectExpression(date);

            if (currencyCodes != null && currencyCodes.Length > 1)
                exp = exp.AndAlso(x => currencyCodes.Contains(x.CurrencyCode));

            return DbSet
                .Where(exp)
                .OrderBy(x => x.CurrencyCode);
        }

        public virtual async Task<Dtos.ExchangeRateSummary<PK>[]> GetTodayOrSpecificDate(DateTimeOffset? specificDate = null, string[]? currencyCodes = null, CancellationToken cancellationToken = default)
        {
            return await GetTodayOrSpecificDateQuery(
                    specificDate: specificDate,
                    currencyCodes: currencyCodes
                )
                .Select(x => new Dtos.ExchangeRateSummary<PK>
                {
                    CurrencyCode = x.CurrencyCode,
                    BanknoteSelling = x.BanknoteSelling,
                    Id = x.Id,
                })
                .ToArrayAsync(cancellationToken);
        }
    }
}
