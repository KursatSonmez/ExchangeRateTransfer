using System;

namespace ExchangeRateTransfer.Utilities
{
    public interface ITransferSettings
    {
        /// <summary>
        /// Dosya yolunun tamamını temsil eder
        /// 
        /// Varsayılan: "CurrentDirectory + AuditFileName"
        /// </summary>
        string? AuditFilePath { get; set; }

        /// <summary>
        /// Uzantısı ile birlikte adını temsil eder
        /// 
        /// Varsayılan: audit_exchange_rate.txt
        /// </summary>
        string? AuditFileName { get; set; }

        /// <summary>
        /// Audit dosyasına yazma işleminin yapılıp yapılmayacağını temsil eder
        /// </summary>
        bool AuditIsActive { get; set; }

        /// <summary>
        /// Servisin günlük çalışma saati (24 saat üzerinden).
        /// 2 haneli saat değeri yazılmalıdır.
        /// 
        /// Örneğin 23 yazıldıysa, saat 23 iken servisin tetiklediği herhangi bir dakikada,
        /// saat 23 dilimindeyken çalışacaktır.
        /// 
        /// Format: "00", "23", "13" vs...
        /// Varsayılan: 00
        /// </summary>
        string? WorkingHour { get; set; }

        /// <summary>
        /// Servisin hangi zaman aralığında çalışacağını temsil eder.
        /// 
        /// Varsayılan: 30 Dakikada bir çalıştırılır.
        /// </summary>
        TimeSpan TimerPeriod { get; set; }

        /// <summary>
        /// Eğer true ise:
        /// <br />
        /// Servisin günlük çalışma saatini dikkate almaz.
        /// Saate bakılmaksızın çalışması için kullanılır.
        /// <br />
        /// Varsayılan: false
        /// </summary>
        public bool SkipWorkingHour { get; set; }
    }
}
