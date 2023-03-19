using ExchangeRateTransfer.DemoWebApp.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateTransfer.UnitTests.Base
{
    public class TestBase
    {
        protected static ILogger<T> Logger<T>() => new NullLoggerFactory().CreateLogger<T>();

        protected static DemoWebAppDbContext NewDbContext(string? databaseName = null)
        {
            databaseName ??= Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<DemoWebAppDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            return DemoWebAppDbContext.Create(options);
        }

        protected virtual string GetDatabaseName([System.Runtime.CompilerServices.CallerMemberName] string? methodName = null)
            => string.Format("{0}.{1}", this.GetType().Name, methodName);
    }
}
