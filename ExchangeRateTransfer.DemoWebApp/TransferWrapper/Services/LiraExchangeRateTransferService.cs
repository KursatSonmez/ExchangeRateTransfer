using ExchangeRateTransfer.DemoWebApp.DataContext;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;

namespace ExchangeRateTransfer.DemoWebApp.TransferWrapper.Services
{
	public interface ILiraExchangeRateTransferService : DotNet.Services.Abstract.ILiraExchangeRateTransferService
	{

	}

	public class LiraExchangeRateTransferService : DotNet.Services.Concrete.LiraExchangeRateTransferService<ExchangeRate, long>, ILiraExchangeRateTransferService
	{
		public LiraExchangeRateTransferService(
				ExchangeRateReader.ILiraExchangeRateReader reader,
				DemoWebAppDbContext dbContext
			) : base(reader, dbContext)
		{
		}
	}
}
