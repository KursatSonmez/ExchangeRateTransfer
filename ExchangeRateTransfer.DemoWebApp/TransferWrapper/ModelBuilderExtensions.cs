using ExchangeRateTransfer.DotNet.Data.EfContext;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DemoWebApp.TransferWrapper
{
    public static class ModelBuilderExtensions
    {
        public static void UseExchangerateTransfer(this ModelBuilder modelBuilder)
            => modelBuilder.UseExchangeRateTransferBuilder<long>();
    }
}
