using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DotNet.Data.EfContext
{
    public static class ModelBuilderExtensions
    {
        public static void UseExchangeRateTransferBuilder<PK>(this ModelBuilder modelBuilder) where PK : struct
        {
            var map = new ExchangeRateMap<PK>();

            modelBuilder.ApplyConfiguration(map);
        }
    }
}
