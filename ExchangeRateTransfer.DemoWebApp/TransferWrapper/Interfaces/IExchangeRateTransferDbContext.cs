using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;

namespace ExchangeRateTransfer.DemoWebApp.TransferWrapper.Interfaces
{
    public interface IExchangeRateTransferDbContext : DotNet.Data.EfContext.IExchangeRateTransferDbContext<ExchangeRate, long>
    {
    }
}
