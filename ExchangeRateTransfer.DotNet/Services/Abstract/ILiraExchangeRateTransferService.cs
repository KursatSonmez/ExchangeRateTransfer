namespace ExchangeRateTransfer.DotNet.Services.Abstract
{
    public interface ILiraExchangeRateTransferService
    {
		/// <summary>
		/// TCMB Web XML sayfasını okur ve aldığı verileri veri tabanına kaydeder.
		/// 
		/// <strong>Eğer ilgili tarihe ait veri varsa herhangi bir işlem yapmaz</strong>
		/// </summary>
		/// <param name="exchangeRateDate">Hangi tarihe ait verilerin getirileceğini belirleyen parametredir</param>
		/// <param name="cancellationToken"></param>
		/// <returns>
		/// Kaydedilen veri sayısını döner
		/// </returns>
		Task<int> ReadAndSaveIfNotAsync(DateTimeOffset exchangeRateDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// TCMB Web XML sayfasını okur ve aldığı verileri veri tabanına kaydeder.
        /// 
        /// <strong>Eğer ilgili tarihe ait veri varsa herhangi bir işlem yapmaz</strong>
        /// </summary>
        /// <param name="exchangeRateDate">Hangi tarihe ait verilerin getirileceğini belirleyen parametredir</param>
        /// <returns>
        /// Kaydedilen veri sayısını döner.
        /// </returns>
        int ReadAndSaveIfNot(DateTimeOffset exchangeRateDate);

        /// <summary>
        /// Tarihe ait kur bilgilerinin olup olmadığını kontrol eder
        /// </summary>
        /// <param name="exchangeRateDate">Hangi tarihe ait verilerin kontrol edileceğini belirleyen parametredir</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Eğer bu tarihe ait veri varsa true, yoksa false döner.
        /// </returns>
        Task<bool> AnyAsync(DateTimeOffset exchangeRateDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tarihe ait kur bilgilerinin olup olmadığını kontrol eder
        /// </summary>
        /// <param name="exchangeRateDate">Hangi tarihe ait verilerin kontrol edileceğini belirleyen parametredir</param>
        /// <returns>
        /// Eğer bu tarihe ait veri varsa true, yoksa false döner.
        /// </returns>
        bool Any(DateTimeOffset exchangeRateDate);
    }
}
