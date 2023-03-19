using ExchangeRateTransfer.DotNet.Data.EntitiesAbstract;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateTransfer.DotNet.Data.Entities
{
    public abstract class RootEntity<PK> : IRootEntity<PK> where PK : struct
    {
        [Key]
        public PK Id { get; set; }

        [Required]
        public DateTimeOffset CreateDate { get; set; }
    }
}
