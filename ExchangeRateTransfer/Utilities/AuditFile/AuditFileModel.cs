using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ExchangeRateTransfer.Utilities.AuditFile
{
    public class AuditFileModel
    {
        [JsonIgnore]
        private DateTimeOffset _AuditDate = DateTimeOffset.Now;

        [JsonProperty(Order = 1)]
        public DateTimeOffset AuditDate
        {
            get => _AuditDate;
            set => _AuditDate = value;
        }

        /// <summary>
        /// Döviz kuru bilgilerinin alındığını tarihi temsil eder (Aktarım tarihi değil, hangi güne ait olduğu tarih)
        /// </summary>
        [JsonProperty("D", Order = 2)]
        public DateTimeOffset? ExchangeRateDate { get; set; }

        [JsonProperty("T", Order = 3)]
        [JsonConverter(typeof(StringEnumConverter))]
        public AuditType? AuditType { get; set; }

        [JsonProperty("S", Order = 4)]
        [JsonConverter(typeof(StringEnumConverter))]
        public AuditStatus AuditStatus { get; set; }

        [JsonProperty("M", Order = 5)]
        public string? Message { get; set; }
    }

    /// <summary>
    /// log tipini temsil eder.
    /// Eğer Daily ise günlük kur alımını temsil eder.
    /// 
    /// SpecificDate ise özel bir tarihe ait alımı temsil eder.
    /// </summary>
    public enum AuditType : byte
    {
        Daily,
        //SpecificDate, Aktif değil
    }

    public enum AuditStatus : byte
    {
        Success,
        Error,
        //Info,
        //Warning,
    }
}
