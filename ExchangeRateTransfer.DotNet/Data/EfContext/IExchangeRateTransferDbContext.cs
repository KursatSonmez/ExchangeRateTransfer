using ExchangeRateTransfer.DotNet.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DotNet.Data.EfContext
{
    public interface IExchangeRateTransferDbContext<T, PK> : IDisposable
        where T : ExchangeRate<PK>
        where PK : struct
    {
        DbSet<T> ExchangeRates { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}
