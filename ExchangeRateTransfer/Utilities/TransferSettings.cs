using System;
using System.IO;

namespace ExchangeRateTransfer.Utilities
{
    public class TransferSettings : ITransferSettings
    {
        public string? AuditFilePath { get; set; }

        public string? AuditFileName { get; set; }

        public bool AuditIsActive { get; set; }

        public TimeSpan TimerPeriod { get; set; }

        private string? _WorkingHour;
        public string? WorkingHour
        {
            get => _WorkingHour;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(WorkingHour));

                //(value.Contains(":") == false && value.Length == 2)
                //    value += ":00"

                if (value.Length != 2)
                    throw new ArgumentException("Must have 2 characters!", nameof(WorkingHour));

                if (!int.TryParse(value, out int res) || (res < 0 || res > 23))
                    throw new ArgumentOutOfRangeException(nameof(WorkingHour), "It can only be in the range [00-23]");

                // 00:00 or 23:50 etc. format control.
                //(Regex.IsMatch(value, "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$") == false)
                //    throw new ArgumentException("Format invalid!", nameof(WorkingHour))

                _WorkingHour = value;
            }
        }
        public bool SkipWorkingHour { get; set; }

        public static TransferSettings LoadDefaultValues()
        {
            var assemblyLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
            return new TransferSettings()
            {
                AuditFileName = "audit_exchange_rate.txt",

                AuditFilePath = Path.Combine(Path.GetDirectoryName(assemblyLocation), "audit_exchange_rate.txt"),

                AuditIsActive = true,

                WorkingHour = "00",

                TimerPeriod = TimeSpan.FromMinutes(30),

                SkipWorkingHour = false,
            };
        }
    }
}
