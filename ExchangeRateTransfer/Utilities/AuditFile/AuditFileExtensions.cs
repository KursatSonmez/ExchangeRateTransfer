using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ExchangeRateTransfer.Common;

namespace ExchangeRateTransfer.Utilities.AuditFile
{
    public static class AuditFileExtensions
    {
        private readonly static object _auditFileLocker = new object();

        /// <summary>
        /// Eğer audit log dosyası oluşturulması gerekiyorsa oluşturur
        /// </summary>
        /// <param name="settings"></param>
        public static void CreateAuditFile(ITransferSettings settings)
        {
            if (!settings.AuditIsActive)
                return;

            if (File.Exists(settings.AuditFilePath))
                return;

            lock (_auditFileLocker)
                if (!File.Exists(settings.AuditFilePath))
                {
                    using var _ = File.Create(settings.AuditFilePath);
                }
        }

        /// <summary>
        /// Audit dosyasına log yazar
        /// </summary>
        /// <param name="exchangeRateDate"></param>
        /// <param name="isSpecificDate">Belirli bir tarih ya da günlük bir işlem olup olmadığını temsil eder</param>
        /// <param name="code">Tanımlayıcı mesaj kodu</param>
        /// <param name="message">Loga yazılacak mesaj içeriği</param>
        /// <returns></returns>
        public static void AddAuditLine(this AuditFileModel audit, ITransferSettings settings, CancellationToken cancellationToken = default)
        {
            StringBuilder s = new StringBuilder(audit.ToJson());

            char[] chars = s.ToString().ToCharArray();

            Console.WriteLine($"[ExchangeRateFactory]   {DateTimeOffset.Now.dd_MM_yyyy_HH_mm_ss()}   {s}");

            if (!settings.AuditIsActive)
                return;

            lock (_auditFileLocker)
            {
                using StreamWriter tw = new StreamWriter(settings.AuditFilePath, append: true, encoding: Encoding.UTF8);

                tw.WriteLineAsync(chars, cancellationToken).Wait(cancellationToken);
            }
        }

        /// <summary>
        /// Log dosyasından son
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static AuditFileModel? GetLastAudit(ITransferSettings settings, CancellationToken cancellationToken = default)
        {
            if (!settings.AuditIsActive)
                return null;

            string? lastLine;

            lock (_auditFileLocker)
                lastLine = FileExtensions.ReadLastLine(settings.AuditFilePath!, cancellationToken: cancellationToken);

            if (lastLine == null)
                return null;

            return JsonExtensions.FromJson<AuditFileModel>(lastLine);
        }
    }
}
