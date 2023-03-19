using ExchangeRateTransfer.Common;
using ExchangeRateTransfer.DotNet.Data.Entities;
using ExchangeRateTransfer.DotNet.Services.Abstract;
using ExchangeRateTransfer.Utilities;
using ExchangeRateTransfer.Utilities.AuditFile;
using Microsoft.Extensions.Logging;

namespace ExchangeRateTransfer.Worker.Workers
{
    public class ExchangeRateTransferWorker<T, PK>
        where T : ExchangeRate<PK>
        where PK : struct
    {
        protected readonly ILogger<ExchangeRateTransferWorker<T, PK>> _logger;
        protected readonly ITransferSettings _settings;
        protected readonly ILiraExchangeRateTransferService _liraExchangeRateTransferService;

        public ExchangeRateTransferWorker(
            ILogger<ExchangeRateTransferWorker<T, PK>> logger,
            ITransferSettings transferSettings,
            ILiraExchangeRateTransferService liraExchangeRateTransferService
            )
        {
            _logger = logger;
            _settings = transferSettings;
            _liraExchangeRateTransferService = liraExchangeRateTransferService;
        }

        protected AuditFileModel? LastAction = null;

        public virtual void Execute(CancellationToken cancellationToken = default)
        {
            SetLastActionFromAuditFile(cancellationToken);

            var now = DateTimeOffset.Now;

            if (!CheckTransferStart(now))
                return;

            string? statusText = LastAction != null && LastAction.AuditStatus != AuditStatus.Success
                ? $"({LastAction.AuditStatus.ToString().ToUpper()})"
                : null;

            Console.WriteLine($"[ExchangeRateFactory]   {DateTimeOffset.Now.dd_MM_yyyy_HH_mm_ss()}   [Begin] Transfer begins! (CurrentDate | LastActionDate = {now.dd_MM_yyyy()} | {LastAction?.ExchangeRateDate.dd_MM_yyyy()}) {statusText}");

            // If the time is appropriate and the exchange rate information for today is not received, it is retrieved and recorded.
            ReadAndSaveIfNot(now, AuditType.Daily);
        }

        private bool CheckTransferStart(DateTimeOffset now)
        {
            var hh = now.ToString("HH");

            // If it is not working hours, no action will be taken.
            if (!SkipWorkingHour && _settings.WorkingHour != hh)
            {
                Console.WriteLine($"[ExchangeRateFactory]   {DateTimeOffset.Now.dd_MM_yyyy_HH_mm_ss()}   [WorkingHour] Transfer will not start (CurrentHour | WorkingHour = {hh} | {_settings.WorkingHour})");
                if (LastAction?.AuditStatus == AuditStatus.Error)
                {
                    string lastActionJson = LastAction.ToJson();
                    _logger.LogError("The last exchange attempt is incorrect! Audit = {LastActionJson}", lastActionJson);
                }
                return false;
            }

            // If the last transaction belongs to this day and was successful,
            // today's currency data is already taken. No action is taken
            if (LastAction?.ExchangeRateDate.HasValue == true
                && now.Date == LastAction.ExchangeRateDate.Value.Date
                && LastAction.AuditStatus == AuditStatus.Success)
            {
                Console.WriteLine($"[ExchangeRateFactory]   {DateTimeOffset.Now.dd_MM_yyyy_HH_mm_ss()}   [LastAction] Transfer will not start (CurrentDate | LastActionDate = {now.dd_MM_yyyy()} | {LastAction.ExchangeRateDate.dd_MM_yyyy()})");
                return false;
            }

            return true;
        }

        private void ReadAndSaveIfNot(DateTimeOffset exchangeRateDate, AuditType auditType)
        {
            try
            {
                var count = _liraExchangeRateTransferService.ReadAndSaveIfNot(exchangeRateDate);

                AuditFileModel audit = new()
                {
                    AuditDate = DateTimeOffset.Now,
                    AuditStatus = AuditStatus.Success,
                    AuditType = auditType,
                    ExchangeRateDate = exchangeRateDate,
                    Message = $"{count} data successfully saved.",
                };

                LastAction = audit;
                audit.AddAuditLine(_settings);
            }
            catch (Exception ex)
            {
                var innerText = ex.InnerException != null
                    ? string.Format(", InnerException = {0}", ex.InnerException.Message)
                    : null;

                AuditFileModel audit = new()
                {
                    AuditDate = DateTimeOffset.Now,
                    AuditStatus = AuditStatus.Error,
                    AuditType = auditType,
                    ExchangeRateDate = exchangeRateDate,
                    Message = $"Error = {ex.Message}{innerText}",
                };

                LastAction = audit;
                audit.AddAuditLine(_settings);
                throw;
            }
        }

        /// <summary>
        /// Audit dosyasından son satır bilgisini getirerek <see cref="LastAction"/> nesnesini doldurur
        /// </summary>
        protected void SetLastActionFromAuditFile(CancellationToken cancellationToken) => LastAction = AuditFileExtensions.GetLastAudit(_settings, cancellationToken);

        /// <summary>
        /// Audit dosyasını oluşturur (Eğer aktif ise)
        /// </summary>
        /// <returns></returns>
        protected void CreateAuditFile() => AuditFileExtensions.CreateAuditFile(_settings);

        /// <summary>
        /// Çalışma saati kotrolünü yapıp yapmayacağını belirler.
        /// Eğer true ise çalışma saati kontrolü yapılmamalıdır
        /// False ise yapılmalıdır.
        /// </summary>
        protected bool SkipWorkingHour
            => _settings.SkipWorkingHour || Environment.GetEnvironmentVariable(EnvironmentNames.ExchangeRateTransfer_Skip_WorkingHour)?.Equals("true", comparisonType: StringComparison.InvariantCultureIgnoreCase) == true;
    }
}
