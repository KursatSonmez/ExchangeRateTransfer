using ExchangeRateTransfer.DotNet.Data.EntitiesAbstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateTransfer.DotNet.Data.Entities
{
    public class ExchangeRate<PK> : RootEntity<PK>, IExchangeRate<PK> where PK : struct
    {
        [Column(TypeName = "Date")]
        [Required]
        public DateTime ExchangeRateDate { get; set; }

        [Column(TypeName = "Date")]
        [Required]
        public DateTime ReleaseDate { get; set; }

        public string? BulletinNo { get; set; }

        public string? Kod { get; set; }

        [Required]
        public string? CurrencyCode { get; set; }

        public int Unit { get; set; }
        public string? Isim { get; set; }

        [Required]
        public string? CurrencyName { get; set; }

        public decimal ForexBuying { get; set; }

        public decimal ForexSelling { get; set; }

        public decimal BanknoteBuying { get; set; }

        public decimal BanknoteSelling { get; set; }

        public decimal? CrossRateUSD { get; set; }

        public decimal? CrossRateOther { get; set; }

        public virtual void OnInsert()
        {
            CreateDate = DateTimeOffset.Now;

            if (typeof(PK) == typeof(Guid))
                Id = (PK)System.ComponentModel.TypeDescriptor.GetConverter(typeof(PK)).ConvertFromInvariantString(Guid.NewGuid().ToString())!;
        }
    }
}
